using FSI.Common.DTO;
using FSI.Common.ETO;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using Google.Api.Gax;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace FSI.Application.EventHandle
{

    public class UpdateProjectRequestStartuperInfoHandle : IDistributedEventHandler<UpdateProjectRequestStartuperInfoEto>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ProjectRequestStartuperInfo, Guid> _projectRequestStartuperInfoRepository;
        private readonly IStartuperRepository _startuperRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IDistributedCache<List<ProjectSimilarStartuper>> _projectSimilarStartuperCache;
        private readonly IConfiguration Configuration;

        public UpdateProjectRequestStartuperInfoHandle(IUnitOfWorkManager unitOfWorkManager, IRepository<ProjectRequestStartuperInfo, Guid> projectRequestStartuperInfoRepository, IRepository<ProjectUser, Guid> projectUserRepository, IStartuperRepository startuperRepository, IDistributedCache<List<ProjectSimilarStartuper>> projectSimilarStartuperCache, IConfiguration configuration)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _projectRequestStartuperInfoRepository = projectRequestStartuperInfoRepository;
            _projectUserRepository = projectUserRepository;
            _startuperRepository = startuperRepository;
            _projectSimilarStartuperCache = projectSimilarStartuperCache;
            Configuration = configuration;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(UpdateProjectRequestStartuperInfoEto eventData)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var pjRqInfo = await _projectRequestStartuperInfoRepository.FindAsync(x => x.ProjectId.Equals(eventData.ProjectId));

                if(pjRqInfo != null)
                {
                    TranslationClient client = TranslationClient.CreateFromApiKey(Configuration["GoogleTranslateKey"]);

                    var fields = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.Fields);
                    var jobs = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.Jobs);
                    var personalities = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.Personalities);
                    var skills = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.Skills);
                    var locations = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.Locations);
                    var yearOfExps = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.YearOfExps);
                    var availableTimes = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, pjRqInfo.AvailableTimes);

                    var describe = client.TranslateText(pjRqInfo.Describe ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var speciality = client.TranslateText(pjRqInfo.Speciality ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var activity = client.TranslateText(pjRqInfo.Activity ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var certificateAndAward = client.TranslateText(pjRqInfo.CertificateAndAward ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var workingExperience = client.TranslateText(pjRqInfo.WorkingExperience ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var workingPlace = client.TranslateText(pjRqInfo.WorkingPlace ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                    var engText = $"{jobs} {locations} {workingPlace} {fields} {speciality} {personalities} {skills} {workingExperience} {activity} {certificateAndAward} {describe} {yearOfExps} {availableTimes}";
                    engText = Regex.Replace(engText, @"\t|\n|\r", " ");
                    pjRqInfo.EngText = engText;

                    var startupers = await _startuperRepository.GetQueryableAsync();
                    var projectUserIds = (await _projectUserRepository.GetListAsync(x => x.ProjectId.Equals(eventData.ProjectId))).Select(x => x.UserId);

                    var newStartupers = startupers.Where(x => !projectUserIds.Contains(x.Id)).ToList();

                    var mlContext = new MLContext();
                    var sentenceDatas = newStartupers.Select(x =>
                    {
                        return new SentenceData()
                        {
                            Sentence = x.StartuperEnglishText
                        };
                    }).ToList();

                    sentenceDatas.Add(new SentenceData { Sentence = engText });

                    var dataView = mlContext.Data.LoadFromEnumerable(sentenceDatas);

                    var textFeaturizer = mlContext.Transforms.Text.FeaturizeText("Features", new TextFeaturizingEstimator.Options
                    {
                        OutputTokensColumnName = "Tokens"
                    }, "Sentence");
                    var transformedData = textFeaturizer.Fit(dataView).Transform(dataView);
                    var features = mlContext.Data.CreateEnumerable<FeatureData>(transformedData, reuseRowObject: false);

                    var myFeatures = features.ElementAt(sentenceDatas.Count - 1).Features;
                    List<ProjectSimilarStartuper> similarities = new List<ProjectSimilarStartuper>();
                    for (var i = 0; i < sentenceDatas.Count - 1; i++)
                    {
                        var startuperId = newStartupers[i].Id;
                        var startuperFeatures = features.ElementAt(i).Features;
                        var similarity = SimilarityUtil.CalculateCosineSimilarity(myFeatures, startuperFeatures);

                        similarities.Add(new ProjectSimilarStartuper()
                        {
                            Similarity = (float)similarity,
                            StartuperId = startuperId
                        });
                    }
                    pjRqInfo.Similarities = similarities;
                    await _projectRequestStartuperInfoRepository.UpdateAsync(pjRqInfo);

                    // lưu mảng startuperSimilar của project vào cache với key là projectId
                    await _projectSimilarStartuperCache.SetAsync(eventData.ProjectId.ToString(), similarities, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    });
                }

                await uow.CompleteAsync();
            }
        }
    }
}

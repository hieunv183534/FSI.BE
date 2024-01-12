using FSI.Common.DTO;
using FSI.Common.ETO;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace FSI.Application.EventHandle
{
    public class UpdateStartuperInfoHandle : IDistributedEventHandler<UpdateStartuperInfoEto>, ITransientDependency
    {
        private readonly IStartuperRepository _startuperRepository;
        private readonly IRepository<StartuperSimilarity, Guid> _startuperSimilarityRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConfiguration Configuration;

        public UpdateStartuperInfoHandle(IStartuperRepository startuperRepository, IRepository<StartuperSimilarity, Guid> startuperSimilarityRepository, IUnitOfWorkManager unitOfWorkManager, IConfiguration configuration)
        {
            _startuperRepository = startuperRepository;
            _startuperSimilarityRepository = startuperSimilarityRepository;
            _unitOfWorkManager = unitOfWorkManager;
            Configuration = configuration;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(UpdateStartuperInfoEto eventData)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {

                TranslationClient client = TranslationClient.CreateFromApiKey(Configuration["GoogleTranslateKey"]);


                var startupers = await _startuperRepository.GetListAsync();
                var myStartuper = startupers.FirstOrDefault(x => x.Id.Equals(eventData.StartuperId));

                var age = (int)((DateTime.Now - myStartuper.DateOfBirth).TotalDays / 365.242199) + " years old ";
                var job = DataPointDto.GetSingleEnglish(FsiDataValue.Jobs, myStartuper.Job);
                var location = DataPointDto.GetSingleEnglish(FsiDataValue.Areas, myStartuper.Location);
                var workingPlace = client.TranslateText(myStartuper.WorkingPlace ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var field = DataPointDto.GetSingleEnglish(FsiDataValue.Fields, myStartuper.Field.Value);
                var speciality = client.TranslateText(myStartuper.Speciality ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var personality = DataPointDto.GetMultiEnglish(FsiDataValue.Personalities, myStartuper.Personality);
                var skill = DataPointDto.GetMultiEnglish(FsiDataValue.Skills, myStartuper.Skill);
                var workingExperience = client.TranslateText(myStartuper.WorkingExperience ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var activity = client.TranslateText(myStartuper.Activity ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var certificateAndAward = client.TranslateText(myStartuper.CertificateAndAward ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var describe = client.TranslateText(myStartuper.Describe ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var yearOfExp = DataPointDto.GetSingleEnglish(FsiDataValue.YearOfExps, myStartuper.YearOfExp.Value);
                var availableTime = DataPointDto.GetSingleEnglish(FsiDataValue.AvailableTimes, myStartuper.AvailableTime.Value);
                var infoString = $"{age} {job} {job} {job} {location} {workingPlace} {field} {field} {field} {speciality} {personality} {skill} {workingExperience} {activity} {certificateAndAward} {describe} {yearOfExp} {availableTime}";
                myStartuper.StartuperEnglishText = Regex.Replace(infoString, @"\t|\n|\r", " ");
                await _startuperRepository.UpdateAsync(myStartuper);


                var mlContext = new MLContext();
                var sentenceDatas = startupers.Select(x =>
                {
                    return new SentenceData()
                    {
                        Sentence = x.StartuperEnglishText
                    };
                }).ToList();
                var dataView = mlContext.Data.LoadFromEnumerable(sentenceDatas);

                var textFeaturizer = mlContext.Transforms.Text.FeaturizeText("Features", new TextFeaturizingEstimator.Options
                {
                    OutputTokensColumnName = "Tokens"
                }, "Sentence");

                var transformedData = textFeaturizer.Fit(dataView).Transform(dataView);
                var features = mlContext.Data.CreateEnumerable<FeatureData>(transformedData, reuseRowObject: false);

                var myIndex = startupers.FindIndex(x => x.Id.Equals(eventData.StartuperId));
                var myFeatures = features.ElementAt(myIndex).Features;

                var startuperSimilarities = await _startuperSimilarityRepository.GetListAsync(x => x.UserId.Equals(eventData.StartuperId) || x.TargetId.Equals(eventData.StartuperId));

                List<StartuperSimilarity> listInsert = new List<StartuperSimilarity>();
                List<StartuperSimilarity> listUpdate = new List<StartuperSimilarity>();

                for (var i = 0; i < sentenceDatas.Count; i++)
                {
                    if (i != myIndex)
                    {
                        var tartgetId = startupers[i].Id;
                        var targetFeatures = features.ElementAt(i).Features;
                        var cosineSimilarity = SimilarityUtil.CalculateCosineSimilarity(myFeatures, targetFeatures);
                        var similar1 = startuperSimilarities.Find(x => x.UserId.Equals(eventData.StartuperId) && x.TargetId.Equals(tartgetId));
                        var similar2 = startuperSimilarities.Find(x => x.TargetId.Equals(eventData.StartuperId) && x.UserId.Equals(tartgetId));
                        if (similar1 == null)
                        {
                            listInsert.Add(new StartuperSimilarity()
                            {
                                UserId = eventData.StartuperId,
                                TargetId = tartgetId,
                                Similarity = (float)cosineSimilarity
                            });
                        }
                        else
                        {
                            similar1.Similarity = (float)cosineSimilarity;
                            listUpdate.Add(similar1);
                        }

                        if (similar2 == null)
                        {
                            listInsert.Add(new StartuperSimilarity()
                            {
                                TargetId = eventData.StartuperId,
                                UserId = tartgetId,
                                Similarity = (float)cosineSimilarity
                            });
                        }
                        else
                        {
                            similar2.Similarity = (float)cosineSimilarity;
                            listUpdate.Add(similar2);
                        }
                    }
                }

                await _startuperSimilarityRepository.InsertManyAsync(listInsert);
                await _startuperSimilarityRepository.UpdateManyAsync(listUpdate);

                await uow.CompleteAsync();
            }
        }

    }
}

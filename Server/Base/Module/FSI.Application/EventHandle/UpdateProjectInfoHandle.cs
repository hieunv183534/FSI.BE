using FSI.Common.ETO;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using Google.Api.Gax;
using Google.Cloud.Translation.V2;
using Microsoft.ML.Transforms.Text;
using Microsoft.ML;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using FSI.Common.DTO;
using Microsoft.Extensions.Configuration;

namespace FSI.Application.EventHandle
{
    public class UpdateProjectInfoHandle : IDistributedEventHandler<UpdateProjectInfoEto>, ITransientDependency
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectSimilarity, Guid> _projectSimilarityRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConfiguration Configuration;

        public UpdateProjectInfoHandle(IUnitOfWorkManager unitOfWorkManager, IRepository<ProjectSimilarity, Guid> projectSimilarityRepository, IProjectRepository projectRepository, IConfiguration configuration)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _projectSimilarityRepository = projectSimilarityRepository;
            _projectRepository = projectRepository;
            Configuration = configuration;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(UpdateProjectInfoEto eventData)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                TranslationClient client = TranslationClient.CreateFromApiKey(Configuration["GoogleTranslateKey"]);

                var projects = await _projectRepository.GetListAsync();
                var myProject= projects.FirstOrDefault(x => x.Id.Equals(eventData.ProjectId));

                var name = client.TranslateText(myProject.ProjectName ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var description = client.TranslateText(myProject.Description ?? "", LanguageCodes.English, LanguageCodes.Vietnamese, TranslationModel.NeuralMachineTranslation).TranslatedText;
                var fields = DataPointDto.GetMultiEnglish(FsiDataValue.Fields, myProject.Fields);
                var stage = myProject.Stage.ToString();
                var area = DataPointDto.GetSingleEnglish(FsiDataValue.Areas, myProject.Area.Value);

                var infoString = $"{name} {description} {fields} {fields} {fields} {stage} {stage} {area}";
                myProject.ProjectEnglishText = Regex.Replace(infoString, @"\t|\n|\r", " ");
                await _projectRepository.UpdateAsync(myProject);

                var mlContext = new MLContext();
                var sentenceDatas = projects.Select(x =>
                {
                    return new SentenceData()
                    {
                        Sentence = x.ProjectEnglishText
                    };
                }).ToList();

                var dataView = mlContext.Data.LoadFromEnumerable(sentenceDatas);
                var textFeaturizer = mlContext.Transforms.Text.FeaturizeText("Features", new TextFeaturizingEstimator.Options
                {
                    OutputTokensColumnName = "Tokens"
                }, "Sentence");

                var transformedData = textFeaturizer.Fit(dataView).Transform(dataView);
                var features = mlContext.Data.CreateEnumerable<FeatureData>(transformedData, reuseRowObject: false);

                var myIndex = projects.FindIndex(x => x.Id.Equals(eventData.ProjectId));
                var myFeatures = features.ElementAt(myIndex).Features;

                var projectSimilarities = await _projectSimilarityRepository.GetListAsync(x => x.ProjectId.Equals(eventData.ProjectId) || x.ProjectTargetId.Equals(eventData.ProjectId));

                List<ProjectSimilarity> listInsert = new List<ProjectSimilarity>();
                List<ProjectSimilarity> listUpdate = new List<ProjectSimilarity>();

                for (var i = 0; i < sentenceDatas.Count; i++)
                {
                    if (i != myIndex)
                    {
                        var tartgetId = projects[i].Id;
                        var targetFeatures = features.ElementAt(i).Features;
                        var cosineSimilarity = SimilarityUtil.CalculateCosineSimilarity(myFeatures, targetFeatures);
                        var similar1 = projectSimilarities.Find(x => x.ProjectId.Equals(eventData.ProjectId) && x.ProjectTargetId.Equals(tartgetId));
                        var similar2 = projectSimilarities.Find(x => x.ProjectTargetId.Equals(eventData.ProjectId) && x.ProjectId.Equals(tartgetId));
                        if (similar1 == null)
                        {
                            listInsert.Add(new ProjectSimilarity()
                            {
                                ProjectId = eventData.ProjectId,
                                ProjectTargetId = tartgetId,
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
                            listInsert.Add(new ProjectSimilarity()
                            {
                                ProjectTargetId = eventData.ProjectId,
                                ProjectId = tartgetId,
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

                await _projectSimilarityRepository.InsertManyAsync(listInsert);
                await _projectSimilarityRepository.UpdateManyAsync(listUpdate);

                await uow.CompleteAsync();
            }
        }
    }
}

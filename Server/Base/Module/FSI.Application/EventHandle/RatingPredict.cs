using FSI.Domain.MatrixRating;
using FSI.Domain.Startuper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.EventHandle
{
    public class RatingPredictClass
    {
        public static List<PredictRatingProject> PredictRating(List<UserProjectRating>? allUserRatings , List<StartuperSimilarity>? myUserSimilarities, List<Guid>? projectIds, int? k)
        {
            var predictRatingProjects = new List<PredictRatingProject>();

            foreach (var projectId in projectIds)
            {
               // lấy danh sách những user đã  rating cho project
               var usersRatingThisProject = allUserRatings.Where(x=> x.ProjectId == projectId).ToList();

                if (!usersRatingThisProject.Any())
                {
                    predictRatingProjects.Add(new PredictRatingProject { ProjectId = projectId, PredictRating = 0.0f });
                    continue;
                }

                // lấy top k user tương tự từ usersRatingThisProject
                var topUserRatingSimilar = usersRatingThisProject.Join(myUserSimilarities, x => x.UserId, y => y.TargetId, (x, y) =>
                {
                    return new
                    {
                        Rating = x,
                        Similar = y
                    };
                }).OrderByDescending(x=> x.Similar.Similarity).Take(k.Value).ToList();
  

                // Tính trung bình rating của các user tương tự cho dự án cụ thể
                var numerator = 0.0f;
                var denominator = 0.0f;

                if(topUserRatingSimilar.Any())
                {
                    numerator = topUserRatingSimilar.Sum(x => x.Similar.Similarity * x.Rating.Rating);
                    denominator = topUserRatingSimilar.Sum(x => Math.Abs(x.Similar.Similarity));
                }

                if (denominator == 0.0f)
                {
                    predictRatingProjects.Add(new PredictRatingProject { ProjectId = projectId, PredictRating = 0.0f });
                    continue;
                }

                var predictedRating = numerator / denominator;
                predictRatingProjects.Add(new PredictRatingProject { ProjectId = projectId, PredictRating = predictedRating });
            }

            return predictRatingProjects;
        }
    }

    public class PredictRatingProject
    {
        public Guid ProjectId { get; set; }
        public float PredictRating { get; set; }
    }
}

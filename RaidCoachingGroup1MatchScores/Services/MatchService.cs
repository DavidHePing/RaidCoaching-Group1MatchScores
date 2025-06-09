using RaidCoachingGroup1MatchScores.Models;
using RaidCoachingGroup1MatchScores.Repositories;
using RaidCoachingGroup1MatchScores.Responses;

namespace RaidCoachingGroup1MatchScores.Services
{
    public class MatchService(IMatchRepository matchRepository) : IMatchService
    {
        public string UpdateMatchScore(UpdateMatchScoresModel model)
        {
            // Get match score from DB
            var matchScoreModel = matchRepository.GetMatchScore(model.MatchId);

            ////validate matchScoreModel
            //matchScoreModel isNull? throw exception
            if (matchScoreModel == null)
            {
                throw new ArgumentException($"Match with ID {model.MatchId} does not exist");
            }

            //convert
            var matchScoreDto = new MatchScoreDto(matchScoreModel);
            
            //validate
            matchScoreDto.IsValidate(model.MatchEvent);
            
            matchScoreDto.UpdateScores(model.MatchEvent);
            
            matchRepository.UpdateMatchScore(new UpdateMatchScoresRepoModel
            {
                MatchId = model.MatchId,
                Scores = matchScoreDto.OriginScores
            });

            ////Update the data into repo
            //convert to dto (done)
            //Validate (done)
            //update (done)
            //return result in format

            return matchScoreDto.DisplayScore();
        }
    }
}
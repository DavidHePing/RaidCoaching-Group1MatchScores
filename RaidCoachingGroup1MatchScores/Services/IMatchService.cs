using RaidCoachingGroup1MatchScores.Models;

namespace RaidCoachingGroup1MatchScores.Services
{
    public interface IMatchService
    {
        public string UpdateMatchScore(UpdateMatchScoresModel model);
    }
}
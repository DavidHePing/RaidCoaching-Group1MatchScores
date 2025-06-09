using RaidCoachingGroup1MatchScores.Models;
using RaidCoachingGroup1MatchScores.Request;

namespace RaidCoachingGroup1MatchScores.Service
{
    public interface IMatchService
    {
        public string UpdateMatchScore(UpdateMatchScoresModel model);
    }
}
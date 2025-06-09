using RaidCoachingGroup1MatchScores.Models;
using RaidCoachingGroup1MatchScores.Request;

namespace RaidCoachingGroup1MatchScores.Repository
{
    public interface IMatchRepository
    {
        public MatchScoreModel GetMatchScore(int matchId);
        public void UpdateMatchScore(UpdateMatchScoresRepoModel model);
    }

    public class MatchScoreModel
    {
        public string Score { get; set; }
    }
}
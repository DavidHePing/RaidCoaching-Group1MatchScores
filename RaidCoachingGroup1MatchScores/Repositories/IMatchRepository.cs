using RaidCoachingGroup1MatchScores.Models;

namespace RaidCoachingGroup1MatchScores.Repositories
{
    public interface IMatchRepository
    {
        public MatchScoreModel GetMatchScore(int matchId);
        public void UpdateMatchScore(UpdateMatchScoresRepoModel model);
    }
}
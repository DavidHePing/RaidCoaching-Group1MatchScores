using RaidCoachingGroup1MatchScores.Enum;
using RaidCoachingGroup1MatchScores.Models;

namespace RaidCoachingGroup1MatchScores.Request;

public class UpdateMatchScoresRequest
{
    public int MatchId { get; set; }
    public MatchEventEnum MatchEvent { get; set; }

    public UpdateMatchScoresModel GetUpdateMatchScoresModel()
    {
        return new UpdateMatchScoresModel()
        {
            MatchId = MatchId,
            MatchEvent = MatchEvent
        };
    }
}
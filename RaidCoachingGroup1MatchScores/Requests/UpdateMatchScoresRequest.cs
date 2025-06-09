using RaidCoachingGroup1MatchScores.Enums;
using RaidCoachingGroup1MatchScores.Models;

namespace RaidCoachingGroup1MatchScores.Requests;

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
using RaidCoachingGroup1MatchScores.Enum;

namespace RaidCoachingGroup1MatchScores.Models;

public class UpdateMatchScoresModel
{
    public int MatchId { get; set; }
    public MatchEventEnum MatchEvent { get; set; }
}
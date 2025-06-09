using RaidCoachingGroup1MatchScores.Enums;

namespace RaidCoachingGroup1MatchScores.Models;

public class MatchScoreModel
{
    public string Score { get; set; }

    public void ValidateMatchScores(UpdateMatchScoresModel model)
    {
        // Check if match exists
        try
        {
            if (this == null)
            {
                throw new ArgumentException($"Match with ID {model.MatchId} does not exist");
            }

            // Parse current score
            var scores = this.Score.Split('-');
            var homeScore = int.Parse(scores[0]);
            var awayScore = int.Parse(scores[1]);

            
            
            // Validate cancel conditions
            if (model.MatchEvent == MatchEventEnum.CancelHomeGoal || model.MatchEvent == MatchEventEnum.CancelAwayGoal)
            {

                // No cancellations when score is 0:0
                if (homeScore == 0 && awayScore == 0)
                {
                    throw new InvalidOperationException("Cannot cancel goals when score is 0:0");
                }

                // No home cancellation when score is H:A
                if (model.MatchEvent == MatchEventEnum.CancelHomeGoal && homeScore == 0)
                {
                    throw new InvalidOperationException("Cannot cancel home goal when home score is 0");
                }

                // No away cancellation when score is A:H
                if (model.MatchEvent == MatchEventEnum.CancelAwayGoal && awayScore == 0)
                {
                    throw new InvalidOperationException("Cannot cancel away goal when away score is 0");
                }
            }
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
        {
            throw new ArgumentException($"Error validating match with ID {model.MatchId}: {ex.Message}");
        }
    }
}
using RaidCoachingGroup1MatchScores.Enums;

namespace RaidCoachingGroup1MatchScores.Models
{
    public class MatchScoreDto
    {
        private readonly string _lastGoal;
        private PeriodEnum DisplayPeriod { get; set; }

        public MatchScoreDto(MatchScoreModel matchScoreModel)
        {
            //"" when 0:0
            _lastGoal = matchScoreModel.Score.Replace(";", string.Empty).LastOrDefault().ToString();
            // Count total home and away goals across both periods
            HomeScore = matchScoreModel.Score.Count(c => c == 'H');
            AwayScore = matchScoreModel.Score.Count(c => c == 'A');

            // Set the display period based on whether we're in second half
            DisplayPeriod = IsSecondHalf(matchScoreModel.Score) ? PeriodEnum.SecondHalf : PeriodEnum.FirstHalf;

            OriginScores = matchScoreModel.Score;
        }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string OriginScores { get; set; }

        public string DisplayScore()
        {
            return $"{HomeScore}:{AwayScore} ({DisplayPeriod.ToDisplayString()})";
        }

        public void IsValidate(MatchEventEnum matchEvent)
        {
            if (string.IsNullOrEmpty(_lastGoal) &&
                matchEvent is MatchEventEnum.CancelHomeGoal or MatchEventEnum.CancelAwayGoal)
            {
                throw new Exception("Cannot cancel goal when no goals have been scored");
            }

            if (matchEvent == MatchEventEnum.CancelHomeGoal && _lastGoal == "A")
            {
                throw new Exception("Cannot cancel home goal when last goal was away");
            }

            if (matchEvent == MatchEventEnum.CancelAwayGoal && _lastGoal == "H")
            {
                throw new Exception("Cannot cancel away goal when last goal was home");
            }

            if (DisplayPeriod == PeriodEnum.SecondHalf && matchEvent == MatchEventEnum.NextPeriod)
            {
                throw new Exception("Cannot advance to next period when already in second half");
            }
        }

        public void UpdateScores(MatchEventEnum matchEventEnum)
        {
            switch (matchEventEnum)
            {
                case MatchEventEnum.HomeGoal:
                    HomeScore++;
                    OriginScores += "H";
                    break;
                case MatchEventEnum.AwayGoal:
                    AwayScore++;
                    OriginScores += "A";
                    break;
                case MatchEventEnum.CancelHomeGoal:
                    HomeScore--;
                    // Handle cases with and without semicolon
                    OriginScores = OriginScores.Remove(OriginScores.LastIndexOf('H'), 1);
                    break;
                case MatchEventEnum.CancelAwayGoal:
                    AwayScore++;
                    // Handle cases with and without semicolon
                    OriginScores = OriginScores.Remove(OriginScores.LastIndexOf('A'), 1);
                    break;
                case MatchEventEnum.NextPeriod:
                    DisplayPeriod = PeriodEnum.SecondHalf;
                    OriginScores += ";";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchEventEnum), matchEventEnum, null);
            }
        }

        private bool IsSecondHalf(string scores)
        {
            return scores.Contains(";");
        }
    }
}
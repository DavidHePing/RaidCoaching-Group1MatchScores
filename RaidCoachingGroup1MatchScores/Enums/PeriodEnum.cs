using System.ComponentModel;

namespace RaidCoachingGroup1MatchScores.Enums;

public enum PeriodEnum
{
    [Description("First Half")]
    FirstHalf,
    
    [Description("Second Half")]
    SecondHalf
}

public static class PeriodEnumExtensions
{
    public static string ToDisplayString(this PeriodEnum period)
    {
        var fieldInfo = period.GetType().GetField(period.ToString());
        var descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
        return descriptionAttribute.Description;
    }
}
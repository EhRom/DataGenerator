
namespace DataGenerator.Domain.Models;

public class Period(DateOnly startPeriod, DateOnly endPeriod, bool isWholeYear) : IPeriod
{
    public DateOnly StartPeriod { get; init; } = startPeriod;

    public DateOnly EndPeriod { get; init; } = endPeriod;

    public bool IsWholeYear { get; init; } = isWholeYear;

    public string StartPeriodText => IsWholeYear ? StartPeriod.Year.ToString() : StartPeriod.ToString();

    public string EndPeriodText => IsWholeYear ? EndPeriod.Year.ToString() : EndPeriod.ToString();

    public static Period CreateNew(DateOnly startPeriod, DateOnly endPeriod, bool isWholeYear)
    {
        if (startPeriod > endPeriod)
            (startPeriod, endPeriod) = (endPeriod, startPeriod);

        endPeriod = isWholeYear ? new DateOnly(endPeriod.Year, 12, 31) : endPeriod;

        return new Period(startPeriod, endPeriod, isWholeYear);
    }
}

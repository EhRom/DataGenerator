namespace DataGenerator.Domain.Models;

public interface IPeriod
{
    DateOnly StartPeriod { get; }

    DateOnly EndPeriod { get; }

    bool IsWholeYear { get; }

    string StartPeriodText { get; }

    string EndPeriodText { get; }
}

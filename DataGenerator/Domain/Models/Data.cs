namespace DataGenerator.Domain.Models;

public abstract class Data : IData
{
    public DateOnly Date { get; init; }

    public bool IsHoliday { get; init; }

    public string HolidayName { get; init; }

    public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

    public Data(DateOnly date, bool isHoliday, string holidayName)
    {
        Date = date;
        IsHoliday = isHoliday;
        HolidayName = holidayName ?? string.Empty;
    }

    public override string ToString()
    {
        return IsHoliday ? $"{Date} ({HolidayName})-{IsWeekend}-{base.ToString()}" : $"{Date}-{IsWeekend}-{base.ToString()}";
    }

    public abstract string GetHeader(char separator);

    public abstract string GetContent(char separator);
}

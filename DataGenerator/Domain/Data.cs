namespace DataGenerator.Domain;

public class Data : IData
{
    public DateOnly Date { get; init; }

    public bool IsHoliday { get; init; }

    public string HolidayName { get; init; }

    public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

    public Data(DateOnly date, bool isHoliday, string holidayName)
    {
        Date = date;
        IsHoliday = isHoliday;
        HolidayName = holidayName;
    }

    public static IData CreateNew(DateOnly date, bool isHoliday, string holidayName)
    {
        return new Data(date, isHoliday, holidayName);
    }

    public override string ToString()
    {
        return IsHoliday ? $"{Date} ({HolidayName})-{IsWeekend}-{base.ToString()}" : $"{Date}-{IsWeekend}-{base.ToString()}";
    }
}

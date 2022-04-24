namespace DataGenerator.Domain;

public class ProductivityData : Data, IData
{
    public string ProductName { get; init; } = string.Empty;

    public decimal Volume { get; init; }

    public decimal PeopleTime { get; init; }

    public ProductivityData(DateOnly date, bool isHoliday, string holidayName, string productName, decimal volume, decimal peopleTime)
        : base(date, isHoliday, holidayName)
    {
        ProductName = productName;
        Volume = IsHoliday || IsWeekend ? 0m : volume;
        PeopleTime = IsHoliday || IsWeekend ? 0m : peopleTime;
    }

    public static IData CreateNew(DateOnly date, bool isHoliday, string holidayName, string productName, decimal volume, decimal peopleTime)
    {
        return new ProductivityData(date, isHoliday, holidayName, productName, volume, peopleTime);
    }
}

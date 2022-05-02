namespace DataGenerator.Domain.Products;

public class ProductivityData : Data, IData
{
    public string ProductName { get; init; } = string.Empty;

    public double Volume { get; init; }

    public double PeopleTime { get; init; }

    public ProductivityData(DateOnly date, bool isHoliday, string holidayName, string productName, double volume, double peopleTime)
        : base(date, isHoliday, holidayName)
    {
        ProductName = productName;
        Volume = IsHoliday || IsWeekend ? 0d : volume;
        PeopleTime = IsHoliday || IsWeekend ? 0d : peopleTime;
    }

    public static IData CreateNew(DateOnly date, bool isHoliday, string holidayName, string productName, double volume, double peopleTime)
    {
        return new ProductivityData(date, isHoliday, holidayName, productName, volume, peopleTime);
    }

    public override string GetHeader(char separator)
    {
        string[] headers =  new string[]
        {
            nameof(Date),
            nameof(ProductName),
            nameof(Volume),
            nameof(PeopleTime)
        };

        return string.Join(separator, headers);
    }

    public override string GetContent(char separator)
    {
        string[] contents = new string[]
        {
            Date.ToString(),
            ProductName,
            Volume.ToString(),
            PeopleTime.ToString()

        };
        return string.Join(separator, contents);
    }
}

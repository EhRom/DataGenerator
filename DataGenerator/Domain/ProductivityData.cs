namespace DataGenerator.Domain;

public class ProductivityData : Data, IData
{
    public ProductivityData(DateOnly date, bool isHoliday, string holidayName)
        : base(date, isHoliday, holidayName)
    { }

    public static IData CreateNew(DateOnly date, bool isHoliday, string holidayName)
    {
        return new ProductivityData(date, isHoliday, holidayName);
    }
}


namespace DataGenerator.Domain
{
    public interface IData
    {
        DateOnly Date { get; init; }

        bool IsWeekend { get; }

        string HolidayName { get; init; }

        bool IsHoliday { get; init; }
    }
}
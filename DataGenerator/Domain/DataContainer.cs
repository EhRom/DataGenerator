using DataGenerator.Domain.Holidays;

namespace DataGenerator.Domain;

public class DataContainer
{
    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public IEnumerable<Holiday> Holidays { get; init; }

    public IDictionary<DateOnly, IData> GeneratedData { get; init; }

    public DataContainer(IEnumerable<Holiday> holidays, DateOnly startDate, DateOnly endDate)
    {
        Holidays = holidays;
        StartDate = startDate;
        EndDate = endDate;

        GeneratedData = new Dictionary<DateOnly, IData>();
    }

    public static DataContainer CreateNew(IEnumerable<Holiday> holidays, DateOnly startDate, DateOnly endDate)
    {
        return new DataContainer(holidays, startDate, endDate);
    }

    public void AddData(IData data)
    {
        GeneratedData[data.Date] = data;
    }
}

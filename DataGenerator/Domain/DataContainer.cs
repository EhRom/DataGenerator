using DataGenerator.Domain.Holidays;
using System.Security.Cryptography;
using System.Text;

namespace DataGenerator.Domain;

public class DataContainer : IDisposable
{
    private bool disposed = false;

    private readonly RandomNumberGenerator randomNumberGenerator;

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public IEnumerable<Holiday> Holidays { get; init; }

    public IDictionary<DateOnly, ICollection<IData>> GeneratedData { get; init; }

    public DataContainer(IEnumerable<Holiday> holidays, DateOnly startDate, DateOnly endDate)
    {
        Holidays = holidays;
        StartDate = startDate;
        EndDate = endDate;

        GeneratedData = new Dictionary<DateOnly, ICollection<IData>>();
        randomNumberGenerator = RandomNumberGenerator.Create();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects).
                randomNumberGenerator.Dispose();
            }

            disposed = true;
        }
    }

    public static DataContainer CreateNew(IEnumerable<Holiday> holidays, DateOnly startDate, DateOnly endDate)
    {
        return new DataContainer(holidays, startDate, endDate);
    }

    public void AddData(IData data)
    {
        if (!GeneratedData.ContainsKey(data.Date))
            GeneratedData[data.Date] = new List<IData>();

        GeneratedData[data.Date].Add(data);
    }

    public decimal GetRandomValue(decimal defaultValue, long valueVariation, long valueVaraitionDivisor)
    {
        valueVariation = Math.Abs(valueVariation);

        decimal randomValue = (decimal)NextRandomValue(Math.Abs(valueVariation) * -1, Math.Abs(valueVariation)) / valueVaraitionDivisor;

        return defaultValue + randomValue;
    }

    public long NextRandomValue(long minValue, long maxExclusiveValue)
    {
        if (minValue >= maxExclusiveValue)
            return minValue;

        long diff = maxExclusiveValue - minValue;
        long upperBound = long.MaxValue / diff * diff;

        long randomNumber;
        do
        {
            randomNumber = GetRandomLong();
        } while (randomNumber >= upperBound);

        return minValue + (randomNumber % diff);
    }

    private long GetRandomLong()
    {
        byte[] randomBytes = GenerateRandomBytes(sizeof(long));
        return BitConverter.ToInt64(randomBytes, 0);
    }

    private byte[] GenerateRandomBytes(int bytesNumber)
    {
        var buffer = new byte[bytesNumber];
        randomNumberGenerator.GetBytes(buffer, 0, bytesNumber);
        return buffer;
    }

    public string GetCsvContent()
    {
        bool isHeaderSet = false;
        StringBuilder contentBuilder = new StringBuilder();

        foreach (ICollection<IData> generatedDataCollection in GeneratedData.Values)
        {
            foreach (IData generatedData in generatedDataCollection)
            {
                if(!isHeaderSet)
                {
                    contentBuilder.AppendLine(generatedData.GetHeader(';'));
                    isHeaderSet = true;
                }

                contentBuilder.AppendLine(generatedData.GetContent(';'));
            }
        }

        return contentBuilder.ToString();
    }
}

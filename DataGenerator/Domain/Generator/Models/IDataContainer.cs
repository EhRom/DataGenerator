using DataGenerator.Domain.Calendar.Models;
using System.Text;

namespace DataGenerator.Domain.Generator.Models;

public interface IDataContainer : IDisposable
{
    event EventHandler<DataCollectionEventArgs>? SaveData;

    IPeriod Period { get; }

    IEnumerable<Holiday> Holidays { get; }

    long RowCount { get; }

    void AddData(IData data);

    long GetRandomLongValue(long valueVariation);

    long GetRandomLongValue(long defaultValue, long valueVariation);

    double GetRandomDoubleValue(double defaultValue, long valueVariation, long valueVaraitionDivisor);

    long NextRandomValue(long minValue, long maxExclusiveValue);

    bool IsHolidayOrWeekend(DateOnly date);

    string GetCsvContent(char csvSeparatorCharacter);

    public static string GenerateCsvLine(IEnumerable<string> fields, char csvSeparatorCharacter)
    {
        StringBuilder contentBuilder = new StringBuilder();

        contentBuilder.AppendLine(string.Join(csvSeparatorCharacter, fields));
        
        return contentBuilder.ToString();
    }

    public static string GetCsvContent(char csvSeparatorCharacter, ICollection<IData> dataCollection)
    {
        StringBuilder contentBuilder = new StringBuilder();

        dataCollection.ToList().ForEach(d => contentBuilder.AppendLine(GenerateCsvLine(d.GetContent(), csvSeparatorCharacter)));

        return contentBuilder.ToString();
    }
}
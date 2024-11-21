
namespace DataGenerator.Infra;

public class HolidayApiToken : IHolidayApiToken
{
    public IDictionary<string, string> GetHeaders()
    {
        return new Dictionary<string, string>();
    }
}
using DataGenerator.Infra;

namespace DataGenerator.Domain.Holidays;

public class HolidayService : IHolidayService
{
    private const string HolidayServiceUri = "https://calendrier.api.gouv.fr/jours-feries/metropole/";
    private readonly IHttpService httpService;

    public HolidayService(IHttpService httpService)
    {
        this.httpService = httpService;
    }

    public async Task<IEnumerable<Holiday>> GetHolidays(DateOnly startDate, DateOnly endDate)
    {
        return await GetHolidays(startDate.Year, endDate.Year);
    }

    public async Task<IEnumerable<Holiday>> GetHolidays(int startYear, int endYear)
    {
        IEnumerable<Holiday> holidays = new List<Holiday>();

        if (startYear > endYear)
        {
            int year = startYear;
            startYear = endYear;
            endYear = year;
        }

        for (int currentYear = startYear; currentYear <= endYear; currentYear++)
        {
            holidays = holidays.Union(await GetHolidays(currentYear));
        }

        return holidays.ToList();
    }

    public async Task<IEnumerable<Holiday>> GetHolidays(int year)
    {
        string baseUri = HolidayServiceUri;
        Uri holidayUri = BuildGetHolidaysUri(baseUri, year);

        Dictionary<string, string> result = await httpService.HttpGetAsync<Dictionary<string, string>>(holidayUri);
        return ConvertHolidays(result);
    }

    private static Uri BuildGetHolidaysUri(string baseUri, int year)
    {
        return new Uri($"{baseUri}/{year}.json");
    }

    private IEnumerable<Holiday> ConvertHolidays(Dictionary<string, string> baseHolidays)
    {
        foreach (string baseHolidayKey in baseHolidays.Keys)
        {
            yield return new Holiday()
            {
                Date = DateOnly.ParseExact(baseHolidayKey, "yyyy-MM-dd"),
                Name = baseHolidays[baseHolidayKey]
            };
        }
    }
}

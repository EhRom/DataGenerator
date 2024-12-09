using Puffix.Rest;

namespace DataGenerator.Infra;

public class HolidayApiQueryInformation(HttpMethod httpMethod, IHolidayApiToken? token, IDictionary<string, string> headers, string baseUri, string queryPath, string queryParameters, string queryContent) :
        QueryInformation<IHolidayApiToken>(httpMethod, token, headers, baseUri, queryPath, queryParameters, queryContent),
        IHolidayApiQueryInformation
{
    public static IHolidayApiQueryInformation CreateNewUnauthenticatedQuery(HttpMethod httpMethod, string apiUri, string queryPath, string queryParameters, string queryContent)
    {
        return new HolidayApiQueryInformation(httpMethod, default, new Dictionary<string, string>(), apiUri, queryPath, queryParameters, queryContent);
    }

    public static IHolidayApiQueryInformation CreateNewAuthenticatedQuery(IHolidayApiToken token, HttpMethod httpMethod, string apiUri, string queryPath, string queryParameters, string queryContent)
    {
        return new HolidayApiQueryInformation(httpMethod, token, new Dictionary<string, string>(), apiUri, queryPath, queryParameters, queryContent);
    }
}

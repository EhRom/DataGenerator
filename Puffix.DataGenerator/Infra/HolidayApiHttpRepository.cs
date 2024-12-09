using Puffix.Rest;

namespace DataGenerator.Infra;

public class HolidayApiHttpRepository(IHttpClientFactory httpClientFactory) :
        RestHttpRepository<IHolidayApiQueryInformation, IHolidayApiToken>(httpClientFactory),
        IHolidayApiHttpRepository
{
    public override IHolidayApiQueryInformation BuildUnauthenticatedQuery(HttpMethod httpMethod, string apiUri, string queryPath, string queryParameters, string queryContent)
    {
        return HolidayApiQueryInformation.CreateNewUnauthenticatedQuery(httpMethod, apiUri, queryPath, queryParameters, queryContent);
    }

    public override IHolidayApiQueryInformation BuildAuthenticatedQuery(IHolidayApiToken token, HttpMethod httpMethod, string apiUri, string queryPath, string queryParameters, string queryContent)
    {
        return HolidayApiQueryInformation.CreateNewAuthenticatedQuery(token, httpMethod, apiUri, queryPath, queryParameters, queryContent);
    }
}

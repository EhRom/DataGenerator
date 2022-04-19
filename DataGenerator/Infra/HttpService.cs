using System.Text.Json;

namespace DataGenerator.Infra;

public class HttpService : IHttpService
{
    private static readonly Func<HttpContent, Task<string>> extracResultAsString = async (httpContent) => await httpContent.ReadAsStringAsync();
    private static readonly Func<HttpContent, Task<Stream>> extracResultAsStream = async (httpContent) => await httpContent.ReadAsStreamAsync();
   
    private readonly IHttpClientFactory httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<string> HttpGetAsync(Uri getQueryUri)
    {
        return await BaseHttpGetAsync(getQueryUri, extracResultAsString);
    }

    public async Task<Stream> HttpGetAsyncAsStream(Uri getQueryUri)
    {
        return await BaseHttpGetAsync(getQueryUri, extracResultAsStream);
    }

    public async Task<ObjectT> HttpGetAsync<ObjectT>(Uri getQueryUri)
        where ObjectT : class
    {
        Func<HttpContent, Task<ObjectT>> extracJsonResult = async (httpContent) =>
        {
            Stream resultStream = await httpContent.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ObjectT>(resultStream);
        };

        return await BaseHttpGetAsync(getQueryUri, extracJsonResult);
    }

    private async Task<BaseResultT> BaseHttpGetAsync<BaseResultT>(Uri getQueryUri, Func<HttpContent, Task<BaseResultT>> extractResult)
    {
        using HttpClient httpClient = httpClientFactory.CreateClient();

        using var response = await httpClient.GetAsync(getQueryUri);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error (calling: {getQueryUri}) {response.StatusCode}: {response.ReasonPhrase} >> {errorContent}");
        }

        return await extractResult(response.Content);
    }
}


namespace DataGenerator.Infra;

public interface IHttpService
{
    Task<string> HttpGetAsync(Uri getQueryUri);

    Task<Stream> HttpGetAsyncAsStream(Uri getQueryUri);

    Task<ObjectT> HttpGetAsync<ObjectT>(Uri getQueryUri)
        where ObjectT : class;
}

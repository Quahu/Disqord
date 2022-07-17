namespace Disqord.Http;

public interface IHttpClientFactory
{
    IHttpClient CreateClient();
}
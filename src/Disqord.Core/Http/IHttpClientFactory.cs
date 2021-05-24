namespace Disqord.Http
{
    public interface IHttpClientFactory
    {
        public const string GlobalName = "Disqord";
        
        IHttpClient CreateClient(string name);
    }
}

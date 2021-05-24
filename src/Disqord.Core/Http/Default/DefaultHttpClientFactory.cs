namespace Disqord.Http.Default
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public System.Net.Http.IHttpClientFactory UnderlyingFactory { get; }

        public DefaultHttpClientFactory(System.Net.Http.IHttpClientFactory underlyingFactory)
        {
            UnderlyingFactory = underlyingFactory;
        }

        public IHttpClient CreateClient(string name)
        {
            var client = UnderlyingFactory.CreateClient(name);
            return new DefaultHttpClient(client);
        }
    }
}

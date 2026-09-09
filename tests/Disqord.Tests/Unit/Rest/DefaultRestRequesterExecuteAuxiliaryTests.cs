using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Rest.Api.Default;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Disqord.Tests.Unit.Rest;

public class DefaultRestRequesterExecuteAuxiliaryTests
{
    private static readonly Uri DownloadUri = new("https://cdn.discordapp.com/attachments/1/2/image.png?ex=1&is=2&hm=3");

    [Test]
    public async Task ExecuteAuxiliaryAsync_AbsoluteUri_SendsRequestOnAuxiliaryClient()
    {
        // Arrange
        var factory = new RecordingHttpClientFactory();
        var requester = CreateRequester(factory);
        using var request = new DefaultHttpRequest(HttpRequestMethod.Get, DownloadUri, null, HttpCompletionMode.HeadersRead);

        // Act
        using var response = await requester.ExecuteAuxiliaryAsync(request);

        // Assert
        Assert.That(factory.Clients, Has.Count.EqualTo(2));
        Assert.That(factory.Clients[0].Requests, Is.Empty);
        Assert.That(factory.Clients[1].Requests, Is.EqualTo(new[]
        {
            request
        }));

        Assert.That(response, Is.SameAs(factory.Clients[1].Response));
    }

    [Test]
    public void Constructor_Always_AuxiliaryClientHasNoBaseUriAndHasUserAgent()
    {
        // Arrange
        var factory = new RecordingHttpClientFactory();

        // Act
        _ = CreateRequester(factory);

        // Assert
        Assert.That(factory.Clients, Has.Count.EqualTo(2));
        Assert.That(factory.Clients[0].BaseUri, Is.Not.Null);
        Assert.That(factory.Clients[1].BaseUri, Is.Null);
        Assert.That(factory.Clients[1].DefaultHeaders["User-Agent"], Is.EqualTo(Library.UserAgent));
    }

    [Test]
    public void Bind_WithBotToken_SetsAuthorizationOnApiClientOnly()
    {
        // Arrange
        var factory = new RecordingHttpClientFactory();
        var requester = CreateRequester(factory);
        var apiClient = new FakeRestApiClient
        {
            Token = Token.Bot("MjM4NDk0NzU2NTIxMzc3Nzky.CunGFQ.wUILz7z6HoJzVeq6pyHPmVgQgV4")
        };

        // Act
        requester.Bind(apiClient);

        // Assert
        Assert.That(factory.Clients[0].DefaultHeaders.ContainsKey("Authorization"), Is.True);
        Assert.That(factory.Clients[1].DefaultHeaders.ContainsKey("Authorization"), Is.False);
    }

    [Test]
    public void ExecuteAuxiliaryAsync_RelativeUri_ThrowsArgumentException()
    {
        // Arrange
        var factory = new RecordingHttpClientFactory();
        var requester = CreateRequester(factory);
        using var request = new DefaultHttpRequest(HttpRequestMethod.Get, new Uri("attachments/1/2/image.png", UriKind.Relative), null, HttpCompletionMode.HeadersRead);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await requester.ExecuteAuxiliaryAsync(request));
        Assert.That(factory.Clients[1].Requests, Is.Empty);
    }

    private static DefaultRestRequester CreateRequester(IHttpClientFactory factory)
    {
        return new DefaultRestRequester(Options.Create(new DefaultRestRequesterConfiguration()), NullLogger<DefaultRestRequester>.Instance, factory);
    }

    private sealed class RecordingHttpClientFactory : IHttpClientFactory
    {
        public List<RecordingHttpClient> Clients { get; } = new();

        public IHttpClient CreateClient()
        {
            var client = new RecordingHttpClient();
            Clients.Add(client);
            return client;
        }
    }

    private sealed class RecordingHttpClient : IHttpClient
    {
        public Uri? BaseUri { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; } = new();

        public List<IHttpRequest> Requests { get; } = new();

        public IHttpResponse Response { get; } = new FakeHttpResponse();

        public void SetDefaultHeader(string name, string value)
        {
            DefaultHeaders[name] = value;
        }

        public Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            Requests.Add(request);
            return Task.FromResult(Response);
        }

        public void Dispose()
        { }
    }
}

using Disqord.Http;
using Disqord.Models;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Unit.Rest;

public class AttachmentDownloadTests
{
    private const string Url = "https://cdn.discordapp.com/attachments/1/2/image.png?ex=1&is=2&hm=3";
    private const string ProxyUrl = "https://media.discordapp.net/attachments/1/2/image.png?ex=1&is=2&hm=3";

    private static readonly byte[] Body = [1, 2, 3, 4, 5];

    [Test]
    public async Task DownloadAsync_Success_ReturnsBody()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        await using var stream = await fixture.Attachment.DownloadAsync();

        // Assert
        Assert.That(await ReadAllAsync(stream), Is.EqualTo(Body));
    }

    [Test]
    public async Task DownloadAsync_StreamDisposed_DisposesResponse()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        var stream = await fixture.Attachment.DownloadAsync();
        await stream.DisposeAsync();

        // Assert
        Assert.That(fixture.Response.IsDisposed, Is.True);
    }

    [Test]
    public async Task DownloadAsync_CancellationTokenOnly_SendsHeadersReadGetForUrlWithoutContent()
    {
        // Arrange
        var fixture = new Fixture();
        using var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await using var stream = await fixture.Attachment.DownloadAsync(cancellationTokenSource.Token);

        // Assert
        var request = fixture.Requester.Request!;
        Assert.That(request.Method, Is.EqualTo(HttpRequestMethod.Get));
        Assert.That(request.Uri, Is.EqualTo(new Uri(Url)));
        Assert.That(request.Content, Is.Null);
        Assert.That(request.CompletionMode, Is.EqualTo(HttpCompletionMode.HeadersRead));
    }

    [Test]
    public async Task DownloadAsync_UseProxyUrl_RequestsProxyUrl()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        await using var stream = await fixture.Attachment.DownloadAsync(useProxyUrl: true);

        // Assert
        Assert.That(fixture.Requester.Request!.Uri, Is.EqualTo(new Uri(ProxyUrl)));
    }

    [Test]
    public void DownloadAsync_NonSuccessStatus_ThrowsAndDisposesResponse()
    {
        // Arrange
        var fixture = new Fixture
        {
            StatusCode = HttpResponseStatusCode.NotFound,
            ReasonPhrase = "Not Found"
        };

        // Act
        var exception = Assert.ThrowsAsync<AttachmentDownloadException>(async () => await fixture.Attachment.DownloadAsync());

        // Assert
        Assert.That(exception!.IsHttpError, Is.True);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpResponseStatusCode.NotFound));
        Assert.That(exception.ReasonPhrase, Is.EqualTo("Not Found"));
        Assert.That(exception.InnerException, Is.Null);
        Assert.That(fixture.Response.IsDisposed, Is.True);
    }

    [Test]
    public void DownloadAsync_RequesterThrows_ThrowsAttachmentDownloadExceptionWrappingIt()
    {
        // Arrange
        var fixture = new Fixture();
        var requesterException = new HttpRequestException("Connection refused.");
        fixture.Requester.ExecuteException = requesterException;

        // Act
        var exception = Assert.ThrowsAsync<AttachmentDownloadException>(async () => await fixture.Attachment.DownloadAsync());

        // Assert
        Assert.That(exception!.IsHttpError, Is.False);
        Assert.That(exception.StatusCode, Is.Null);
        Assert.That(exception.ReasonPhrase, Is.Null);
        Assert.That(exception.InnerException, Is.SameAs(requesterException));
    }

    [Test]
    public void DownloadAsync_CancelledByCaller_ThrowsOperationCanceledExceptionUnwrapped()
    {
        // Arrange
        var fixture = new Fixture();
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        Assert.ThrowsAsync<OperationCanceledException>(async () => await fixture.Attachment.DownloadAsync(cancellationTokenSource.Token));
    }

    [Test]
    public void DownloadToFileAsync_DirectoryDoesNotExist_ThrowsAttachmentDownloadExceptionWrappingIt()
    {
        // Arrange
        var fixture = new Fixture();
        var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "image.png");

        // Act
        var exception = Assert.ThrowsAsync<AttachmentDownloadException>(async () => await fixture.Attachment.DownloadToFileAsync(path));

        // Assert
        Assert.That(exception!.IsHttpError, Is.False);
        Assert.That(exception.StatusCode, Is.Null);
        Assert.That(exception.InnerException, Is.TypeOf<DirectoryNotFoundException>());
    }

    [Test]
    public async Task DownloadToMemoryAsync_Success_ReturnsRewoundStreamWithBody()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        using var memoryStream = await fixture.Attachment.DownloadToMemoryAsync();

        // Assert
        Assert.That(memoryStream.Position, Is.Zero);
        Assert.That(memoryStream.ToArray(), Is.EqualTo(Body));
        Assert.That(fixture.Response.IsDisposed, Is.True);
    }

    [TestCase(2)]
    [TestCase(10)]
    public async Task DownloadToMemoryAsync_AdvertisedFileSizeDiffersFromBodyLength_ReturnsBody(int fileSize)
    {
        // Arrange
        var fixture = new Fixture
        {
            FileSize = fileSize
        };

        // Act
        using var memoryStream = await fixture.Attachment.DownloadToMemoryAsync();

        // Assert
        Assert.That(memoryStream.ToArray(), Is.EqualTo(Body));
    }

    [Test]
    public async Task DownloadToAsync_Success_CopiesBodyToDestination()
    {
        // Arrange
        var fixture = new Fixture();
        using var destination = new MemoryStream();

        // Act
        await fixture.Attachment.DownloadToAsync(destination);

        // Assert
        Assert.That(destination.ToArray(), Is.EqualTo(Body));
        Assert.That(fixture.Response.IsDisposed, Is.True);
    }

    [Test]
    public void DownloadToAsync_NonWritableDestination_ThrowsArgumentExceptionWithoutRequesting()
    {
        // Arrange
        var fixture = new Fixture();
        using var destination = new MemoryStream(Body, false);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await fixture.Attachment.DownloadToAsync(destination));
        Assert.That(fixture.Requester.Request, Is.Null);
    }

    [Test]
    public async Task DownloadToFileAsync_Success_WritesFile()
    {
        // Arrange
        var fixture = new Fixture();
        var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            // Act
            await fixture.Attachment.DownloadToFileAsync(path);

            // Assert
            Assert.That(await File.ReadAllBytesAsync(path), Is.EqualTo(Body));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void DownloadToFileAsync_NonSuccessStatus_DoesNotCreateFile()
    {
        // Arrange
        var fixture = new Fixture
        {
            StatusCode = HttpResponseStatusCode.Forbidden,
            ReasonPhrase = "Forbidden"
        };

        var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            // Act
            Assert.ThrowsAsync<AttachmentDownloadException>(async () => await fixture.Attachment.DownloadToFileAsync(path));

            // Assert
            Assert.That(File.Exists(path), Is.False);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public async Task DownloadAsLocalAttachmentAsync_PlainFileName_CopiesNameDescriptionAndContent()
    {
        // Arrange
        var fixture = new Fixture
        {
            Description = "A picture."
        };

        // Act
        using var localAttachment = await fixture.Attachment.DownloadAsLocalAttachmentAsync();

        // Assert
        Assert.That(localAttachment.FileName.Value, Is.EqualTo("image.png"));
        Assert.That(localAttachment.Description.Value, Is.EqualTo("A picture."));
        Assert.That(localAttachment.IsSpoiler.HasValue, Is.False);
        Assert.That(localAttachment.Id.HasValue, Is.False);
        Assert.That(await ReadAllAsync(localAttachment.Stream.Value), Is.EqualTo(Body));
    }

    [Test]
    public async Task DownloadAsLocalAttachmentAsync_NullDescription_LeavesDescriptionUnset()
    {
        // Arrange
        var fixture = new Fixture();

        // Act
        using var localAttachment = await fixture.Attachment.DownloadAsLocalAttachmentAsync();

        // Assert
        Assert.That(localAttachment.Description.HasValue, Is.False);
    }

    [Test]
    public async Task DownloadAsLocalAttachmentAsync_SpoilerPrefixedFileName_StripsPrefixAndSetsIsSpoiler()
    {
        // Arrange
        var fixture = new Fixture
        {
            FileName = "SPOILER_image.png"
        };

        // Act
        using var localAttachment = await fixture.Attachment.DownloadAsLocalAttachmentAsync();

        // Assert
        Assert.That(localAttachment.FileName.Value, Is.EqualTo("image.png"));
        Assert.That(localAttachment.IsSpoiler.Value, Is.True);
    }

    [Test]
    public async Task DownloadAsLocalAttachmentAsync_SpoilerFlag_SetsIsSpoiler()
    {
        // Arrange
        var fixture = new Fixture
        {
            Flags = AttachmentFlags.Spoiler
        };

        // Act
        using var localAttachment = await fixture.Attachment.DownloadAsLocalAttachmentAsync();

        // Assert
        Assert.That(localAttachment.FileName.Value, Is.EqualTo("image.png"));
        Assert.That(localAttachment.IsSpoiler.Value, Is.True);
    }

    private static async Task<byte[]> ReadAllAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    private sealed class Fixture
    {
        private FakeHttpResponse? _response;
        private FakeRestRequester? _requester;
        private IAttachment? _attachment;

        public byte[] Body { get; init; } = AttachmentDownloadTests.Body;

        public HttpResponseStatusCode StatusCode { get; init; } = HttpResponseStatusCode.OK;

        public string? ReasonPhrase { get; init; } = "OK";

        public string FileName { get; init; } = "image.png";

        public int? FileSize { get; init; }

        public string? Description { get; init; }

        public AttachmentFlags Flags { get; init; }

        public FakeHttpResponse Response => _response ??= new FakeHttpResponse
        {
            StatusCode = StatusCode,
            ReasonPhrase = ReasonPhrase,
            Body = Body
        };

        public FakeRestRequester Requester => _requester ??= new FakeRestRequester(Response);

        public IAttachment Attachment => _attachment ??= CreateAttachment();

        private IAttachment CreateAttachment()
        {
            var model = new AttachmentJsonModel
            {
                Id = new Snowflake(2UL),
                FileName = FileName,
                Size = FileSize ?? Body.Length,
                Url = Url,
                ProxyUrl = ProxyUrl,
                Flags = Flags
            };

            if (Description != null)
            {
                model.Description = Description;
            }

            var apiClient = new FakeRestApiClient
            {
                Requester = Requester
            };

            return new TransientAttachment(new FakeRestClient(apiClient), model);
        }
    }

    private sealed class FakeRestRequester : IRestRequester
    {
        private readonly IHttpResponse _response;

        public IHttpRequest? Request { get; private set; }

        public Exception? ExecuteException { get; set; }

        public IRestApiClient ApiClient => throw new NotSupportedException();

        public int Version => throw new NotSupportedException();

        public IHttpClient HttpClient => throw new NotSupportedException();

        public ILogger Logger => throw new NotSupportedException();

        public FakeRestRequester(IHttpResponse response)
        {
            _response = response;
        }

        public void Bind(IRestApiClient value)
        {
            throw new NotSupportedException();
        }

        public Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<IHttpResponse> ExecuteAuxiliaryAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            Request = request;
            cancellationToken.ThrowIfCancellationRequested();
            if (ExecuteException != null)
            {
                throw ExecuteException;
            }

            return Task.FromResult(_response);
        }
    }
}

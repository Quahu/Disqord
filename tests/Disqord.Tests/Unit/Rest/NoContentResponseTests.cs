using System.Text;
using Disqord.Http;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;
using Disqord.Serialization.Json.Default;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Disqord.Tests.Unit.Rest;

public class NoContentResponseTests
{
    private const string WebhookToken = "webhook-token";

    private static readonly Snowflake ApplicationId = 1;

    private static readonly Snowflake WebhookId = 2;

    private static readonly Snowflake MessageId = 3;

    private static readonly Snowflake EmojiId = 4;

    private static readonly Snowflake EntitlementId = 5;

    private static readonly Snowflake GuildId = 6;

    private static readonly Snowflake UserId = 7;

    [Test]
    public async Task ExecuteWebhookAsync_WithoutWait_ReturnsNull()
    {
        // Arrange
        var apiClient = CreateApiClient(NoContent());
        var content = new ExecuteWebhookJsonRestRequestContent
        {
            Content = "Test"
        };

        // Act
        var model = await apiClient.ExecuteWebhookAsync(WebhookId, WebhookToken, content, wait: false);

        // Assert
        Assert.That(model, Is.Null);
    }

    [Test]
    public async Task ExecuteWebhookAsync_WithWait_ReturnsMessageModel()
    {
        // Arrange
        var apiClient = CreateApiClient(Ok("""{"id":"123","channel_id":"456"}"""));
        var content = new ExecuteWebhookJsonRestRequestContent
        {
            Content = "Test"
        };

        // Act
        var model = await apiClient.ExecuteWebhookAsync(WebhookId, WebhookToken, content, wait: true);

        // Assert
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Id, Is.EqualTo(new Snowflake(123)));
    }

    [Test]
    public void DeleteWebhookMessageAsync_NoContentResponse_DoesNotThrow()
    {
        // Arrange
        var apiClient = CreateApiClient(NoContent());

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await apiClient.DeleteWebhookMessageAsync(WebhookId, WebhookToken, MessageId));
    }

    [Test]
    public void DeleteApplicationEmojiAsync_NoContentResponse_DoesNotThrow()
    {
        // Arrange
        var apiClient = CreateApiClient(NoContent());

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await apiClient.DeleteApplicationEmojiAsync(ApplicationId, EmojiId));
    }

    [Test]
    public void DeleteTestEntitlementAsync_NoContentResponse_DoesNotThrow()
    {
        // Arrange
        var apiClient = CreateApiClient(NoContent());

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await apiClient.DeleteTestEntitlementAsync(ApplicationId, EntitlementId));
    }

    [Test]
    public async Task AddMemberAsync_UserAlreadyMember_ReturnsNull()
    {
        // Arrange
        var apiClient = CreateApiClient(NoContent());
        var content = new AddMemberJsonRestRequestContent("access-token");

        // Act
        var model = await apiClient.AddMemberAsync(GuildId, UserId, content);

        // Assert
        Assert.That(model, Is.Null);
    }

    [Test]
    public async Task AddMemberAsync_UserAdded_ReturnsMemberModel()
    {
        // Arrange
        var apiClient = CreateApiClient(Created("""{"roles":["123"],"deaf":false,"mute":false}"""));
        var content = new AddMemberJsonRestRequestContent("access-token");

        // Act
        var model = await apiClient.AddMemberAsync(GuildId, UserId, content);

        // Assert
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Roles, Is.EqualTo(new[]
        {
            new Snowflake(123)
        }));
    }

    private static FakeHttpResponse NoContent()
    {
        return new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.NoContent,
            ReasonPhrase = "No Content"
        };
    }

    private static FakeHttpResponse Ok(string body)
    {
        return new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.OK,
            ReasonPhrase = "OK",
            Body = Encoding.UTF8.GetBytes(body)
        };
    }

    private static FakeHttpResponse Created(string body)
    {
        return new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.Created,
            ReasonPhrase = "Created",
            Body = Encoding.UTF8.GetBytes(body)
        };
    }

    private static DefaultRestApiClient CreateApiClient(FakeHttpResponse httpResponse)
    {
        var rateLimiter = new FakeRestRateLimiter
        {
            HttpResponse = httpResponse
        };

        return new DefaultRestApiClient(
            Options.Create(new DefaultRestApiClientConfiguration()),
            NullLogger<DefaultRestApiClient>.Instance,
            Token.Bot("MjM4NDk0NzU2NTIxMzc3Nzky.CunGFQ.wUILz7z6HoJzVeq6pyHPmVgQgV4"),
            rateLimiter,
            new FakeRestRequester(),
            new DefaultJsonSerializer());
    }
}

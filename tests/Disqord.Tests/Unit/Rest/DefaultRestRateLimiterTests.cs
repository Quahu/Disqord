using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Disqord.Http;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;
using Disqord.Serialization.Json.Default;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Tests.Unit.Rest;

public class DefaultRestRateLimiterTests
{
    private const string BucketHash = "bucket-hash";

    private static readonly Snowflake FirstChannelId = 1;

    private static readonly Snowflake SecondChannelId = 2;

    private static readonly Snowflake ApplicationId = 3;

    private static readonly Snowflake WebhookId = 4;

    private static readonly TimeSpan HangTimeout = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan ConcurrentWriteTimeout = TimeSpan.FromSeconds(1);

    private static readonly TimeSpan BucketCleanupInterval = TimeSpan.FromMinutes(10);

    [Test]
    public async Task ExecuteAsync_BucketHashLearnedFromAnotherChannelWhileRequestQueued_CompletesQueuedRequest()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var rateLimiter = CreateRateLimiter(requester, new ManualTimeProvider());
        var firstChannelRoute = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var secondChannelRoute = Route.Channel.CreateMessage.Format([SecondChannelId]);

        var failingExchange = requester.Script(firstChannelRoute);
        var queuedExchange = requester.Script(firstChannelRoute);
        var hashTeachingExchange = requester.Script(secondChannelRoute);

        using var failingRequest = new DefaultRestRequest(firstChannelRoute);
        using var queuedRequest = new DefaultRestRequest(firstChannelRoute);
        using var hashTeachingRequest = new DefaultRestRequest(secondChannelRoute);

        var failingTask = rateLimiter.ExecuteAsync(failingRequest, CancellationToken.None);
        var queuedTask = rateLimiter.ExecuteAsync(queuedRequest, CancellationToken.None);
        await failingExchange.Sent.WaitAsync(HangTimeout);

        var hashTeachingTask = rateLimiter.ExecuteAsync(hashTeachingRequest, CancellationToken.None);
        hashTeachingExchange.Respond(CreateBucketResponse());
        await hashTeachingTask.WaitAsync(HangTimeout);

        failingExchange.Fail(new HttpRequestException("Connection reset."));
        await Task.WhenAny(failingTask).WaitAsync(HangTimeout);

        queuedExchange.Respond(CreateBucketResponse());

        // Act
        var response = await queuedTask.WaitAsync(HangTimeout);

        // Assert
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_GlobalRateLimitHit_RetriesAfterBodyRetryAfter()
    {
        // Arrange
        var rateLimitedResponse = CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 0.25, headerRetryAfter: 1);

        // Act
        var (isRetriedEarly, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromMilliseconds(250));

        // Assert
        Assert.That(isRetriedEarly, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_SharedRateLimitHit_RetriesAfterBodyRetryAfter()
    {
        // Arrange
        var rateLimitedResponse = CreateRateLimitedResponse("shared", isGlobal: false, bodyRetryAfter: 0.25, headerRetryAfter: 1, resetAfter: 5);

        // Act
        var (isRetriedEarly, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromMilliseconds(250));

        // Assert
        Assert.That(isRetriedEarly, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_CloudflareRateLimitHit_RetriesAfterHeaderRetryAfter()
    {
        // Arrange
        var rateLimitedResponse = new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.TooManyRequests,
            Body = Encoding.UTF8.GetBytes("<html>Too many requests.</html>")
        };

        rateLimitedResponse.Headers[RestApiHeaderNames.RetryAfter] = "1";

        // Act
        var (isRetriedEarly, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1));

        // Assert
        Assert.That(isRetriedEarly, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_RateLimitHitWithOnlyBodyRetryAfter_RetriesAfterBodyRetryAfter()
    {
        // Arrange
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 0.25, headerRetryAfter: 1);
        rateLimitedResponse.Headers.Remove(RestApiHeaderNames.RetryAfter);

        // Act
        var (isRetriedEarly, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromMilliseconds(250));

        // Assert
        Assert.That(isRetriedEarly, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_RateLimitHitWithBucketHashOfNewRoute_RetriesAfterRetryAfter()
    {
        // Arrange
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);
        rateLimitedResponse.Headers[RestApiHeaderNames.RateLimitBucket] = BucketHash;

        // Act
        var (isRetriedEarly, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1));

        // Assert
        Assert.That(isRetriedEarly, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_RateLimitHitRetried_DisposesRateLimitedResponse()
    {
        // Arrange
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);

        // Act
        await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1));

        // Assert
        Assert.That(rateLimitedResponse.IsDisposed, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_RateLimitHitWithoutRetryAfter_ReturnsResponseWithReadableBody()
    {
        // Arrange
        const string body = "{\"message\":\"You are being rate limited.\",\"code\":0}";
        var requester = new ScriptedRestRequester();
        var rateLimiter = CreateRateLimiter(requester, new ManualTimeProvider());
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);

        using var message = new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent(body)
        };

        message.Headers.TryAddWithoutValidation("Via", "1.1 google");
        await message.Content.LoadIntoBufferAsync();

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(new Disqord.Http.Default.DefaultHttpResponse(message));
        var response = await task.WaitAsync(HangTimeout);

        // Act
        var stream = await response.HttpResponse.ReadAsync(CancellationToken.None);
        string returnedBody;
        using (var reader = new StreamReader(stream))
        {
            returnedBody = await reader.ReadToEndAsync();
        }

        // Assert
        Assert.That(returnedBody, Is.EqualTo(body));
    }

    [Test]
    public async Task CloudflareResetsAt_CloudflareRateLimitActive_ReturnsCloudflareResetWithoutGlobalReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);
        requester.Script(route);

        var rateLimitedResponse = new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.TooManyRequests,
            Body = Encoding.UTF8.GetBytes("<html>Too many requests.</html>")
        };

        rateLimitedResponse.Headers[RestApiHeaderNames.RetryAfter] = "1";

        using var request = new DefaultRestRequest(route);
        _ = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(rateLimitedResponse);
        await timeProvider.TimerScheduled.WaitAsync(HangTimeout);

        // Act
        var cloudflareResetsAt = rateLimiter.CloudflareResetsAt;
        var globalResetsAt = rateLimiter.GlobalResetsAt;
        var isGloballyRateLimited = rateLimiter.IsRateLimited();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isGloballyRateLimited, Is.True);
            Assert.That(cloudflareResetsAt, Is.EqualTo(timeProvider.GetUtcNow() + TimeSpan.FromSeconds(1)));
            Assert.That(globalResetsAt, Is.Null);
        });
    }

    [Test]
    public async Task ExecuteAsync_FollowupsOfDifferentInteractionsWithKnownBucketHash_DoNotQueueBehindEachOther()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var rateLimiter = CreateRateLimiter(requester, new ManualTimeProvider());
        var hashTeachingRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "hash-teaching-token"]);
        var firstInteractionRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var secondInteractionRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "second-token"]);

        var hashTeachingExchange = requester.Script(hashTeachingRoute);
        var firstInteractionExchange = requester.Script(firstInteractionRoute);
        var secondInteractionExchange = requester.Script(secondInteractionRoute);

        using var hashTeachingRequest = new DefaultRestRequest(hashTeachingRoute);
        using var firstInteractionRequest = new DefaultRestRequest(firstInteractionRoute);
        using var secondInteractionRequest = new DefaultRestRequest(secondInteractionRoute);

        var hashTeachingTask = rateLimiter.ExecuteAsync(hashTeachingRequest, CancellationToken.None);
        hashTeachingExchange.Respond(CreateBucketResponse());
        await hashTeachingTask.WaitAsync(HangTimeout);

        _ = rateLimiter.ExecuteAsync(firstInteractionRequest, CancellationToken.None);
        await firstInteractionExchange.Sent.WaitAsync(HangTimeout);

        // Act
        _ = rateLimiter.ExecuteAsync(secondInteractionRequest, CancellationToken.None);

        // Assert
        Assert.That(async () => await secondInteractionExchange.Sent.WaitAsync(HangTimeout), Throws.Nothing);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteWhileGloballyRateLimited_SendsImmediately()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var channelRoute = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var followupRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);

        var globalRateLimited = ExecuteScriptedRequest(requester, rateLimiter, channelRoute);
        requester.Script(channelRoute);
        await globalRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        globalRateLimited.Exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 5, headerRetryAfter: 5));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var followup = ExecuteScriptedRequest(requester, rateLimiter, followupRoute);
        await Task.WhenAny(followup.Exchange.Sent, timeProvider.WaitForScheduledTimerCountAsync(2)).WaitAsync(HangTimeout);

        // Assert
        Assert.That(followup.Exchange.Sent.IsCompleted, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteWhileCloudflareRateLimited_WaitsForCloudflareReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var channelRoute = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var followupRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);

        var cloudflareRateLimited = ExecuteScriptedRequest(requester, rateLimiter, channelRoute);
        requester.Script(channelRoute);
        await cloudflareRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        cloudflareRateLimited.Exchange.Respond(CreateRateLimitedResponse(RateLimitKind.Cloudflare));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var followup = ExecuteScriptedRequest(requester, rateLimiter, followupRoute);
        await Task.WhenAny(followup.Exchange.Sent, timeProvider.WaitForScheduledTimerCountAsync(2)).WaitAsync(HangTimeout);

        // Assert
        Assert.That(followup.Exchange.Sent.IsCompleted, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteHitGlobalRateLimit_RetriesAfterItsRetryAfter()
    {
        // Arrange
        var followupRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var rateLimitedResponse = CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 5, headerRetryAfter: 5);

        // Act
        var (isRetriedEarly, _) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(5), followupRoute);

        // Assert
        Assert.That(isRetriedEarly, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteGlobalRateLimitExceedsMaximumDelay_ThrowsBucketRateLimit()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var rateLimiter = CreateRateLimiter(requester, new ManualTimeProvider());
        var followupRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var exchange = requester.Script(followupRoute);

        var options = new DefaultRestRequestOptions
        {
            MaximumDelayDuration = TimeSpan.FromSeconds(1)
        };

        using var request = new DefaultRestRequest(followupRoute, options: options);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);

        // Act
        exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 5, headerRetryAfter: 5));

        // Assert
        Assert.That(async () => await task.WaitAsync(HangTimeout), Throws.TypeOf<MaximumRateLimitDelayExceededException>()
            .With.Property(nameof(MaximumRateLimitDelayExceededException.Delay)).EqualTo(TimeSpan.FromSeconds(5))
            .And.Property(nameof(MaximumRateLimitDelayExceededException.IsGlobalRateLimit)).False);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteHitGlobalRateLimit_DoesNotDelayOtherInteraction()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var firstInteractionRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var secondInteractionRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "second-token"]);

        var globalRateLimited = ExecuteScriptedRequest(requester, rateLimiter, firstInteractionRoute);
        requester.Script(firstInteractionRoute);
        await globalRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        globalRateLimited.Exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 5, headerRetryAfter: 5));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var otherInteraction = ExecuteScriptedRequest(requester, rateLimiter, secondInteractionRoute);
        await Task.WhenAny(otherInteraction.Exchange.Sent, timeProvider.WaitForScheduledTimerCountAsync(2)).WaitAsync(HangTimeout);

        // Assert
        Assert.That(otherInteraction.Exchange.Sent.IsCompleted, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_InteractionRouteHitGlobalRateLimit_DoesNotDelayOtherRoutes()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var followupRoute = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var channelRoute = Route.Channel.CreateMessage.Format([FirstChannelId]);

        var globalRateLimited = ExecuteScriptedRequest(requester, rateLimiter, followupRoute);
        requester.Script(followupRoute);
        await globalRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        globalRateLimited.Exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 5, headerRetryAfter: 5));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var channelRequest = ExecuteScriptedRequest(requester, rateLimiter, channelRoute);
        await Task.WhenAny(channelRequest.Exchange.Sent, timeProvider.WaitForScheduledTimerCountAsync(2)).WaitAsync(HangTimeout);

        // Assert
        Assert.That(channelRequest.Exchange.Sent.IsCompleted, Is.True);
        Assert.That(rateLimiter.GlobalResetsAt, Is.Null);
    }

    [Test]
    public async Task ExecuteAsync_SharedRateLimitHit_LogsNothingAboveDebug()
    {
        // Arrange
        var logger = new RecordingLogger();
        var rateLimitedResponse = CreateRateLimitedResponse("shared", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);

        // Act
        await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1), logger: logger);

        // Assert
        var requestEntries = logger.Entries.Where(static entry => !entry.Message.StartsWith("Rate-limiting with the time provider"));
        Assert.That(requestEntries.Where(static entry => entry.Level > LogLevel.Debug), Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_TokenRouteRateLimited_DoesNotLogToken(
        [Values] TokenRouteKind tokenRouteKind,
        [Values] RateLimitKind rateLimitKind)
    {
        // Arrange
        const string token = "secret-token";
        var logger = new RecordingLogger(LogLevel.Trace);
        var route = tokenRouteKind == TokenRouteKind.Webhook
            ? Route.Webhook.ExecuteWebhook.Format([WebhookId, token])
            : Route.Interactions.CreateFollowupResponse.Format([ApplicationId, token]);

        var rateLimitedResponse = CreateRateLimitedResponse(rateLimitKind);

        // Act
        await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1), route, logger);

        // Assert
        var requestEntries = logger.Entries.Where(static entry => !entry.Message.StartsWith("Rate-limiting with the time provider"));
        Assert.That(requestEntries, Is.Not.Empty);
        Assert.That(requestEntries.Where(static entry => entry.Message.Contains(token)), Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_RouteTemplateWithEscapedBracesRateLimited_LogsRouteAndRetries()
    {
        // Arrange
        var logger = new RecordingLogger(LogLevel.Trace);
        var route = new Route(HttpRequestMethod.Get, "literal{{brace}}/{0:channel_id}").Format([FirstChannelId]);
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);

        // Act
        var (_, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1), route, logger);

        // Assert
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
        Assert.That(logger.Entries.Select(static entry => entry.Message), Has.Some.Contains("Get|literal{brace}/1"));
    }

    [Test]
    public async Task ExecuteAsync_RouteTemplateWithUnclosedBraceRateLimited_LogsRouteAndRetries()
    {
        // Arrange
        var logger = new RecordingLogger(LogLevel.Trace);
        var parameters = new RouteParameters(new Dictionary<string, object>
        {
            ["thing_id"] = 1
        });

        var route = new FormattedRoute(new Route(HttpRequestMethod.Get, "things/{0:thing_id"), "things/1", parameters);
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);

        // Act
        var (_, response) = await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1), route, logger);

        // Assert
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
        Assert.That(logger.Entries.Select(static entry => entry.Message), Has.Some.Contains("Get|things/{0:thing_id"));
    }

    [Test]
    public async Task ExecuteAsync_RouteParameterWithNewLineRateLimited_LogsPercentEncodedParameter()
    {
        // Arrange
        var logger = new RecordingLogger(LogLevel.Trace);
        var route = new Route(HttpRequestMethod.Get, "things/{0:thing_name}").Format(["first\nsecond"]);
        var rateLimitedResponse = CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);

        // Act
        await ExecuteRateLimitedThenRetriedAsync(rateLimitedResponse, TimeSpan.FromSeconds(1), route, logger);

        // Assert
        Assert.That(logger.Entries.Select(static entry => entry.Message), Has.Some.Contains("Get|things/first%0Asecond"));
        Assert.That(logger.Entries.Select(static entry => entry.Message), Has.None.Contains("first\nsecond"));
    }

    [Test]
    public async Task ExecuteAsync_NonSharedRateLimitsAfterSharedRateLimit_LogsFirstAtInformationThenWarning()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var logger = new RecordingLogger();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, logger);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);

        requester.Script(route).Respond(CreateRateLimitedResponse("shared", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1));
        requester.Script(route).Respond(CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1));
        requester.Script(route).Respond(CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1));
        requester.Script(route).Respond(new FakeHttpResponse());

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);

        // Act
        for (var rateLimitedResponseCount = 0; rateLimitedResponseCount < 3; rateLimitedResponseCount++)
        {
            await timeProvider.TimerScheduled.WaitAsync(HangTimeout);
            timeProvider.Advance(TimeSpan.FromSeconds(1));
        }

        await task.WaitAsync(HangTimeout);

        // Assert
        var nonSharedRateLimitLevels = logger.Entries
            .Where(static entry => entry.Message.StartsWith("Hit a rate-limit on route"))
            .Select(static entry => entry.Level);

        Assert.That(nonSharedRateLimitLevels, Is.EqualTo(new[] { LogLevel.Information, LogLevel.Warning }));
    }

    [Test]
    public async Task ExecuteAsync_FirstRateLimitWithLongRetryAfter_LogsAtInformation()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var logger = new RecordingLogger();
        var rateLimiter = CreateRateLimiter(requester, new ManualTimeProvider(), logger);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        requester.Script(route).Respond(CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 30, headerRetryAfter: 30));
        using var request = new DefaultRestRequest(route);

        // Act
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await Task.WhenAny(task).WaitAsync(HangTimeout);

        // Assert
        var rateLimitLevels = logger.Entries
            .Where(static entry => entry.Message.StartsWith("Hit a rate-limit on route"))
            .Select(static entry => entry.Level);

        Assert.That(rateLimitLevels, Is.EqualTo(new[] { LogLevel.Information }));
    }

    [Test]
    public void Constructor_NonSystemTimeProvider_LogsWarning()
    {
        // Arrange
        var logger = new RecordingLogger();

        // Act
        CreateRateLimiter(new ScriptedRestRequester(), new ManualTimeProvider(), logger);

        // Assert
        Assert.That(logger.Entries.Select(static entry => entry.Level), Is.EqualTo(new[] { LogLevel.Warning }));
    }

    [Test]
    public void Constructor_SystemTimeProvider_LogsNothing()
    {
        // Arrange
        var logger = new RecordingLogger();

        // Act
        CreateRateLimiter(new ScriptedRestRequester(), TimeProvider.System, logger);

        // Assert
        Assert.That(logger.Entries, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_RateLimiterLockedByAnotherThread_RetriesRateLimitedRequest()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var rateLimitedExchange = requester.Script(route);
        var retriedExchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        using var externalLock = new ExternalLock(rateLimiter);

        // Act
        var task = Task.Factory.StartNew(static state =>
        {
            var (executingRateLimiter, executedRequest) = ((DefaultRestRateLimiter, DefaultRestRequest)) state!;
            return executingRateLimiter.ExecuteAsync(executedRequest, CancellationToken.None);
        }, (rateLimiter, request), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap();

        await rateLimitedExchange.Sent.WaitAsync(HangTimeout);
        rateLimitedExchange.Respond(CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1));

        await timeProvider.TimerScheduled.WaitAsync(HangTimeout);
        timeProvider.Advance(TimeSpan.FromSeconds(1));

        await retriedExchange.Sent.WaitAsync(HangTimeout);
        retriedExchange.Respond(new FakeHttpResponse());
        var response = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_GlobalRateLimitAndBucketRefillWhileReadingDelay_DoesNotSendBeforeGlobalReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var gatedTimeProvider = new GatedTimeProvider(timeProvider);
        var rateLimiter = CreateRateLimiter(requester, gatedTimeProvider);
        var bucketTeachingRoute = Route.Channel.DeleteMessage.Format([FirstChannelId, 10]);
        var bucketRefillingRoute = Route.Channel.DeleteMessage.Format([FirstChannelId, 11]);
        var queuedRoute = Route.Channel.DeleteMessage.Format([FirstChannelId, 12]);
        var globalRateLimitedRoute = Route.Channel.CreateMessage.Format([SecondChannelId]);

        var bucketTeachingExchange = requester.Script(bucketTeachingRoute);
        var bucketRefillingExchange = requester.Script(bucketRefillingRoute);
        var queuedExchange = requester.Script(queuedRoute);
        var globalRateLimitedExchange = requester.Script(globalRateLimitedRoute);

        using var bucketTeachingRequest = new DefaultRestRequest(bucketTeachingRoute);
        using var bucketRefillingRequest = new DefaultRestRequest(bucketRefillingRoute);
        using var queuedRequest = new DefaultRestRequest(queuedRoute);
        using var globalRateLimitedRequest = new DefaultRestRequest(globalRateLimitedRoute);

        var bucketTeachingTask = rateLimiter.ExecuteAsync(bucketTeachingRequest, CancellationToken.None);
        var bucketRefillingTask = rateLimiter.ExecuteAsync(bucketRefillingRequest, CancellationToken.None);
        _ = rateLimiter.ExecuteAsync(globalRateLimitedRequest, CancellationToken.None);
        await bucketTeachingExchange.Sent.WaitAsync(HangTimeout);
        await bucketRefillingExchange.Sent.WaitAsync(HangTimeout);
        await globalRateLimitedExchange.Sent.WaitAsync(HangTimeout);

        bucketTeachingExchange.Respond(CreateBucketResponse(remaining: 0, resetAfter: 8));
        await bucketTeachingTask.WaitAsync(HangTimeout);

        gatedTimeProvider.Arm();
        _ = rateLimiter.ExecuteAsync(queuedRequest, CancellationToken.None);
        await gatedTimeProvider.Blocked.WaitAsync(HangTimeout);

        globalRateLimitedExchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 3, headerRetryAfter: 3));
        bucketRefillingExchange.Respond(CreateBucketResponse());
        await Task.WhenAny(Task.WhenAll(bucketRefillingTask, timeProvider.WaitForScheduledTimerCountAsync(1)), Task.Delay(ConcurrentWriteTimeout));

        // Act
        gatedTimeProvider.Release();
        await Task.WhenAny(queuedExchange.Sent, timeProvider.WaitForScheduledTimerCountAsync(2)).WaitAsync(HangTimeout);

        // Assert
        Assert.That(queuedExchange.Sent.IsCompleted, Is.False);
    }

    [Test]
    public async Task IsRateLimited_RateLimiterLockedByAnotherThread_ReturnsFalse()
    {
        // Arrange
        var rateLimiter = CreateRateLimiter(new ScriptedRestRequester(), new ManualTimeProvider());
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        using var externalLock = new ExternalLock(rateLimiter);

        // Act
        var task = Task.Factory.StartNew(static state =>
        {
            var (lockedRateLimiter, checkedRoute) = ((DefaultRestRateLimiter, IFormattedRoute)) state!;
            return (IsGloballyRateLimited: lockedRateLimiter.IsRateLimited(), IsRouteRateLimited: lockedRateLimiter.IsRateLimited(checkedRoute));
        }, (rateLimiter, route), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

        var (isGloballyRateLimited, isRouteRateLimited) = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(isGloballyRateLimited, Is.False);
        Assert.That(isRouteRateLimited, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_GlobalRateLimitEndsBeforeExhaustedBucketResets_WaitsForBucketReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExhaustBucketDuringGlobalRateLimitAsync(requester, rateLimiter, timeProvider, route, bucketResetAfter: 8, globalRetryAfter: 3);

        var queuedExchange = requester.Script(route);
        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await timeProvider.WaitForScheduledTimerCountAsync(2).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(TimeSpan.FromSeconds(8) - TimeSpan.FromTicks(1));
        var isSentBeforeBucketReset = queuedExchange.Sent.IsCompleted;

        timeProvider.Advance(TimeSpan.FromTicks(1));
        await queuedExchange.Sent.WaitAsync(HangTimeout);
        queuedExchange.Respond(new FakeHttpResponse());
        var response = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(isSentBeforeBucketReset, Is.False);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_ExhaustedBucketResetExceedsMaximumDelayDuringShorterGlobalRateLimit_ThrowsWithBucketDelay()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExhaustBucketDuringGlobalRateLimitAsync(requester, rateLimiter, timeProvider, route, bucketResetAfter: 8, globalRetryAfter: 3);

        var options = new DefaultRestRequestOptions
        {
            MaximumDelayDuration = TimeSpan.FromSeconds(5)
        };

        requester.Script(route);
        using var request = new DefaultRestRequest(route, options: options);

        // Act
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(async () => await task.WaitAsync(HangTimeout), Throws.TypeOf<MaximumRateLimitDelayExceededException>()
            .With.Property(nameof(MaximumRateLimitDelayExceededException.Delay)).EqualTo(TimeSpan.FromSeconds(8))
            .And.Property(nameof(MaximumRateLimitDelayExceededException.IsGlobalRateLimit)).False);
    }

    [Test]
    public async Task ExecuteAsync_ExhaustedBucketResetsWithGlobalRateLimitBeyondMaximumDelay_ThrowsGlobalRateLimit()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExhaustBucketDuringGlobalRateLimitAsync(requester, rateLimiter, timeProvider, route, bucketResetAfter: 3, globalRetryAfter: 3);

        var options = new DefaultRestRequestOptions
        {
            MaximumDelayDuration = TimeSpan.FromSeconds(2)
        };

        requester.Script(route);
        using var request = new DefaultRestRequest(route, options: options);

        // Act
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(async () => await task.WaitAsync(HangTimeout), Throws.TypeOf<MaximumRateLimitDelayExceededException>()
            .With.Property(nameof(MaximumRateLimitDelayExceededException.Delay)).EqualTo(TimeSpan.FromSeconds(3))
            .And.Property(nameof(MaximumRateLimitDelayExceededException.IsGlobalRateLimit)).True);
    }

    [Test]
    public async Task ExecuteAsync_GlobalRateLimitStartsWhileWaitingForBucketReset_WaitsForGlobalReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(remaining: 0, resetAfter: 5));
        var queued = ExecuteScriptedRequest(requester, rateLimiter, route);
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        var globalRoute = Route.Channel.CreateMessage.Format([SecondChannelId]);
        var globalRateLimited = ExecuteScriptedRequest(requester, rateLimiter, globalRoute);
        requester.Script(globalRoute);
        await globalRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        globalRateLimited.Exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 8, headerRetryAfter: 8));
        await timeProvider.WaitForScheduledTimerCountAsync(2).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(TimeSpan.FromSeconds(5));
        var isSentBeforeGlobalReset = queued.Exchange.Sent.IsCompleted;

        timeProvider.Advance(TimeSpan.FromSeconds(3));
        await queued.Exchange.Sent.WaitAsync(HangTimeout);

        // Assert
        Assert.That(isSentBeforeGlobalReset, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_SuccessiveWaitsEachWithinMaximumDelay_SendsRequest()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(remaining: 0, resetAfter: 5));

        var options = new DefaultRestRequestOptions
        {
            MaximumDelayDuration = TimeSpan.FromSeconds(6)
        };

        var exchange = requester.Script(route);
        using var request = new DefaultRestRequest(route, options: options);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        var globalRoute = Route.Channel.CreateMessage.Format([SecondChannelId]);
        var globalRateLimited = ExecuteScriptedRequest(requester, rateLimiter, globalRoute);
        requester.Script(globalRoute);
        await globalRateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        globalRateLimited.Exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 8, headerRetryAfter: 8));
        await timeProvider.WaitForScheduledTimerCountAsync(2).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(TimeSpan.FromSeconds(5));
        timeProvider.Advance(TimeSpan.FromSeconds(3));
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(new FakeHttpResponse());
        var response = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task ExecuteAsync_BucketResetNotWholeMillisecondsAway_KeepsWaitingUntilReset()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(remaining: 0, resetAfter: 1));
        var halfMillisecond = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond / 2);
        timeProvider.Advance(halfMillisecond);

        _ = ExecuteScriptedRequest(requester, rateLimiter, route);
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var waitDuration = timeProvider.TimeUntilNextTimer;

        // Assert
        Assert.That(waitDuration, Is.GreaterThanOrEqualTo(TimeSpan.FromSeconds(1) - halfMillisecond));
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterBucketsReset_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(resetAfter: 5));

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterRequestWithoutBucketHeaders_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, new FakeHttpResponse());

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAgainAfterNewRequest_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        await ExecuteWithResponseAsync(requester, rateLimiter, Route.Channel.CreateMessage.Format([FirstChannelId]), new FakeHttpResponse());
        timeProvider.Advance(BucketCleanupInterval);
        await ExecuteWithResponseAsync(requester, rateLimiter, Route.Channel.CreateMessage.Format([SecondChannelId]), new FakeHttpResponse());

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedBeforeLimitedBucketResets_KeepsOnlyLimitedBucket()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(remaining: 0, resetAfter: 900));

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.EqualTo(1));
        Assert.That(rateLimiter.IsRateLimited(route), Is.True);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedWhileRequestInFlightOnUnlimitedBucket_KeepsBucket()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);
        var bucketCount = rateLimiter.BucketCount;

        exchange.Respond(new FakeHttpResponse());
        var response = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(bucketCount, Is.EqualTo(1));
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedWhileRequestInFlightOnLimitedBucket_KeepsBucket()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(resetAfter: 5));
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);
        var bucketCount = rateLimiter.BucketCount;

        exchange.Respond(CreateBucketResponse());
        var response = await task.WaitAsync(HangTimeout);

        // Assert
        Assert.That(bucketCount, Is.EqualTo(1));
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterMovedRequestCompletes_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var hashTeachingExchange = requester.Script(route);
        var movedExchange = requester.Script(route);

        using var hashTeachingRequest = new DefaultRestRequest(route);
        using var movedRequest = new DefaultRestRequest(route);
        var hashTeachingTask = rateLimiter.ExecuteAsync(hashTeachingRequest, CancellationToken.None);
        var movedTask = rateLimiter.ExecuteAsync(movedRequest, CancellationToken.None);
        await hashTeachingExchange.Sent.WaitAsync(HangTimeout);
        hashTeachingExchange.Respond(CreateBucketResponse(resetAfter: 5));
        await hashTeachingTask.WaitAsync(HangTimeout);

        await movedExchange.Sent.WaitAsync(HangTimeout);
        movedExchange.Respond(CreateBucketResponse(resetAfter: 5));
        await movedTask.WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterQueuedRequestCancelled_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var firstExchange = requester.Script(route);
        var lastExchange = requester.Script(route);

        using var cancellationTokenSource = new CancellationTokenSource();
        using var firstRequest = new DefaultRestRequest(route);
        using var cancelledRequest = new DefaultRestRequest(route);
        using var lastRequest = new DefaultRestRequest(route);
        var firstTask = rateLimiter.ExecuteAsync(firstRequest, CancellationToken.None);
        var cancelledTask = rateLimiter.ExecuteAsync(cancelledRequest, cancellationTokenSource.Token);
        var lastTask = rateLimiter.ExecuteAsync(lastRequest, CancellationToken.None);
        await firstExchange.Sent.WaitAsync(HangTimeout);

        await cancellationTokenSource.CancelAsync();
        firstExchange.Respond(new FakeHttpResponse());
        await firstTask.WaitAsync(HangTimeout);

        await lastExchange.Sent.WaitAsync(HangTimeout);
        lastExchange.Respond(new FakeHttpResponse());
        await lastTask.WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(async () => await cancelledTask.WaitAsync(HangTimeout), Throws.InstanceOf<OperationCanceledException>());
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterRequestFailed_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Fail(new HttpRequestException("Connection reset."));
        await task.ContinueWith(static _ => { }, TaskScheduler.Default).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterRequestExceededMaximumDelay_IsZero()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(remaining: 0, resetAfter: 5));

        var options = new DefaultRestRequestOptions
        {
            MaximumDelayDuration = TimeSpan.FromSeconds(1)
        };

        using var request = new DefaultRestRequest(route, options: options);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await task.ContinueWith(static _ => { }, TaskScheduler.Default).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedBeforeUnlimitedBucketRateLimitResets_KeepsBucket()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 900, headerRetryAfter: 900));
        await task.ContinueWith(static _ => { }, TaskScheduler.Default).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.EqualTo(1));
        Assert.That(rateLimiter.IsRateLimited(route), Is.True);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedBeforeInteractionGlobalRateLimitResets_KeepsBucket()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token"]);
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 900, headerRetryAfter: 900));
        await task.ContinueWith(static _ => { }, TaskScheduler.Default).WaitAsync(HangTimeout);

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.EqualTo(1));
    }

    [TestCase(0)]
    [TestCase(-1000)]
    public void Constructor_NonPositiveBucketCleanupInterval_Throws(int bucketCleanupIntervalMilliseconds)
    {
        // Act & Assert
        Assert.That(() => CreateRateLimiter(new ScriptedRestRequester(), new ManualTimeProvider(), bucketCleanupInterval: TimeSpan.FromMilliseconds(bucketCleanupIntervalMilliseconds)),
            Throws.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_BucketCleanupIntervalBelowOneMillisecond_SchedulesCleanupEveryMillisecond()
    {
        // Arrange
        var timeProvider = new ManualTimeProvider();
        using var rateLimiter = CreateRateLimiter(new ScriptedRestRequester(), timeProvider, bucketCleanupInterval: TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond / 2));

        // Act
        timeProvider.Advance(TimeSpan.FromMilliseconds(1));

        // Assert
        Assert.That(timeProvider.TimeUntilNextTimer, Is.EqualTo(TimeSpan.FromMilliseconds(1)));
    }

    [Test]
    public void RateLimiter_UnreferencedWithoutDispose_IsCollectedAndStopsCleanupTimer()
    {
        // Arrange
        var timeProvider = new ManualTimeProvider();
        var rateLimiterReference = CreateUnreferencedRateLimiter(timeProvider);

        // Act
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiterReference.TryGetTarget(out _), Is.False);
        Assert.That(timeProvider.PendingTimerCount, Is.Zero);
    }

    [Test]
    public async Task BucketCount_CleanupIntervalElapsedAfterDispose_KeepsBuckets()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, new FakeHttpResponse());
        rateLimiter.Dispose();

        // Act
        timeProvider.Advance(BucketCleanupInterval);

        // Assert
        Assert.That(rateLimiter.BucketCount, Is.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_BucketRemovedByCleanup_SendsRequest()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, bucketCleanupInterval: BucketCleanupInterval);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse(resetAfter: 5));
        timeProvider.Advance(BucketCleanupInterval);
        var bucketCountBeforeRequest = rateLimiter.BucketCount;

        // Act
        var response = await ExecuteWithResponseAsync(requester, rateLimiter, route, CreateBucketResponse());

        // Assert
        Assert.That(bucketCountBeforeRequest, Is.Zero);
        Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpResponseStatusCode.OK));
    }

    [TestCase(4999, true)]
    [TestCase(5000, false)]
    public async Task IsRateLimited_ExhaustedBucket_ReturnsTrueUntilReset(int elapsedMilliseconds, bool expected)
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var exchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(CreateBucketResponse(remaining: 0, resetAfter: 5));
        await task.WaitAsync(HangTimeout);

        timeProvider.Advance(TimeSpan.FromMilliseconds(elapsedMilliseconds));

        // Act
        var isRateLimited = rateLimiter.IsRateLimited(route);

        // Assert
        Assert.That(isRateLimited, Is.EqualTo(expected));
    }

    [Test]
    public async Task IsRateLimited_NoRouteDuringGlobalRateLimit_ReturnsTrue()
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var route = Route.Channel.CreateMessage.Format([FirstChannelId]);

        var rateLimited = ExecuteScriptedRequest(requester, rateLimiter, route);
        requester.Script(route);
        await rateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        rateLimited.Exchange.Respond(CreateRateLimitedResponse(RateLimitKind.Global));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var isRateLimited = rateLimiter.IsRateLimited();

        // Assert
        Assert.That(isRateLimited, Is.True);
    }

    [TestCase(RateLimitKind.Global, TokenRouteKind.Webhook, true)]
    [TestCase(RateLimitKind.Global, TokenRouteKind.Interaction, false)]
    [TestCase(RateLimitKind.Cloudflare, TokenRouteKind.Webhook, true)]
    [TestCase(RateLimitKind.Cloudflare, TokenRouteKind.Interaction, true)]
    public async Task IsRateLimited_RouteDuringGlobalOrCloudflareRateLimit_ReturnsWhetherRequestWouldWait(RateLimitKind rateLimitKind, TokenRouteKind tokenRouteKind,
        bool expected)
    {
        // Arrange
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider);
        var channelRoute = Route.Channel.CreateMessage.Format([FirstChannelId]);
        var route = tokenRouteKind == TokenRouteKind.Webhook
            ? Route.Webhook.ExecuteWebhook.Format([WebhookId, "webhook-token"])
            : Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "interaction-token"]);

        var rateLimited = ExecuteScriptedRequest(requester, rateLimiter, channelRoute);
        requester.Script(channelRoute);
        await rateLimited.Exchange.Sent.WaitAsync(HangTimeout);
        rateLimited.Exchange.Respond(CreateRateLimitedResponse(rateLimitKind));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);

        // Act
        var isRateLimited = rateLimiter.IsRateLimited(route);

        // Assert
        Assert.That(isRateLimited, Is.EqualTo(expected));
    }

    [Test]
    public void IsRateLimited_InteractionRouteWithNonStringToken_DoesNotThrow()
    {
        // Arrange
        var rateLimiter = CreateRateLimiter(new ScriptedRestRequester(), new ManualTimeProvider());
        var route = Route.Interactions.CreateFollowupResponse.Format([ApplicationId, "first-token".AsMemory()]);

        // Act & Assert
        Assert.That(() => rateLimiter.IsRateLimited(route), Throws.Nothing);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference<DefaultRestRateLimiter> CreateUnreferencedRateLimiter(ManualTimeProvider timeProvider)
    {
        return new WeakReference<DefaultRestRateLimiter>(CreateRateLimiter(new ScriptedRestRequester(), timeProvider, bucketCleanupInterval: BucketCleanupInterval));
    }

    private static (ScriptedExchange Exchange, Task<IRestResponse> Task) ExecuteScriptedRequest(ScriptedRestRequester requester, DefaultRestRateLimiter rateLimiter,
        IFormattedRoute route)
    {
        var exchange = requester.Script(route);
        var task = rateLimiter.ExecuteAsync(new DefaultRestRequest(route), CancellationToken.None);
        return (exchange, task);
    }

    private static async Task<IRestResponse> ExecuteWithResponseAsync(ScriptedRestRequester requester, DefaultRestRateLimiter rateLimiter, IFormattedRoute route,
        IHttpResponse response)
    {
        var exchange = requester.Script(route);
        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await exchange.Sent.WaitAsync(HangTimeout);
        exchange.Respond(response);
        return await task.WaitAsync(HangTimeout);
    }

    private static async Task ExhaustBucketDuringGlobalRateLimitAsync(ScriptedRestRequester requester, DefaultRestRateLimiter rateLimiter, ManualTimeProvider timeProvider,
        IFormattedRoute route, int bucketResetAfter, int globalRetryAfter)
    {
        var bucketExhaustingExchange = requester.Script(route);
        using var bucketExhaustingRequest = new DefaultRestRequest(route);
        var bucketExhaustingTask = rateLimiter.ExecuteAsync(bucketExhaustingRequest, CancellationToken.None);
        await bucketExhaustingExchange.Sent.WaitAsync(HangTimeout);
        bucketExhaustingExchange.Respond(CreateBucketResponse(remaining: 0, resetAfter: bucketResetAfter));
        await bucketExhaustingTask.WaitAsync(HangTimeout);

        var globalRateLimitedRoute = Route.Channel.CreateMessage.Format([SecondChannelId]);
        var globalRateLimitedExchange = requester.Script(globalRateLimitedRoute);
        requester.Script(globalRateLimitedRoute);
        _ = rateLimiter.ExecuteAsync(new DefaultRestRequest(globalRateLimitedRoute), CancellationToken.None);
        await globalRateLimitedExchange.Sent.WaitAsync(HangTimeout);
        globalRateLimitedExchange.Respond(CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: globalRetryAfter, headerRetryAfter: globalRetryAfter));
        await timeProvider.WaitForScheduledTimerCountAsync(1).WaitAsync(HangTimeout);
    }

    private static async Task<(bool IsRetriedEarly, IRestResponse Response)> ExecuteRateLimitedThenRetriedAsync(IHttpResponse rateLimitedResponse, TimeSpan retryAfter,
        IFormattedRoute? route = null, ILogger<DefaultRestRateLimiter>? logger = null)
    {
        var requester = new ScriptedRestRequester();
        var timeProvider = new ManualTimeProvider();
        var rateLimiter = CreateRateLimiter(requester, timeProvider, logger: logger);
        route ??= Route.Channel.CreateMessage.Format([FirstChannelId]);

        var rateLimitedExchange = requester.Script(route);
        var retriedExchange = requester.Script(route);

        using var request = new DefaultRestRequest(route);
        var task = rateLimiter.ExecuteAsync(request, CancellationToken.None);
        await rateLimitedExchange.Sent.WaitAsync(HangTimeout);
        rateLimitedExchange.Respond(rateLimitedResponse);

        await timeProvider.TimerScheduled.WaitAsync(HangTimeout);
        var justBeforeRetryAfter = retryAfter - TimeSpan.FromTicks(1);
        timeProvider.Advance(justBeforeRetryAfter);
        var isRetriedEarly = retriedExchange.Sent.IsCompleted;

        timeProvider.Advance(retryAfter - justBeforeRetryAfter);
        await retriedExchange.Sent.WaitAsync(HangTimeout);
        retriedExchange.Respond(new FakeHttpResponse());

        return (isRetriedEarly, await task.WaitAsync(HangTimeout));
    }

    private static DefaultRestRateLimiter CreateRateLimiter(ScriptedRestRequester requester, TimeProvider timeProvider,
        ILogger<DefaultRestRateLimiter>? logger = null, TimeSpan? bucketCleanupInterval = null)
    {
        var configuration = new DefaultRestRateLimiterConfiguration
        {
            BucketCleanupInterval = bucketCleanupInterval ?? Timeout.InfiniteTimeSpan
        };

        var rateLimiter = new DefaultRestRateLimiter(
            Options.Create(configuration),
            logger ?? NullLogger<DefaultRestRateLimiter>.Instance,
            timeProvider);

        rateLimiter.Bind(new FakeRestApiClient
        {
            Requester = requester,
            Serializer = new DefaultJsonSerializer()
        });

        return rateLimiter;
    }

    private static FakeHttpResponse CreateBucketResponse(int remaining = 4, int resetAfter = 5, int limit = 5)
    {
        var response = new FakeHttpResponse();
        response.Headers[RestApiHeaderNames.RateLimitBucket] = BucketHash;
        response.Headers[RestApiHeaderNames.RateLimitLimit] = limit.ToString(CultureInfo.InvariantCulture);
        response.Headers[RestApiHeaderNames.RateLimitRemaining] = remaining.ToString(CultureInfo.InvariantCulture);
        response.Headers[RestApiHeaderNames.RateLimitResetAfter] = resetAfter.ToString(CultureInfo.InvariantCulture);
        return response;
    }

    private static FakeHttpResponse CreateRateLimitedResponse(RateLimitKind rateLimitKind)
    {
        switch (rateLimitKind)
        {
            case RateLimitKind.User:
            {
                return CreateRateLimitedResponse("user", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);
            }
            case RateLimitKind.Shared:
            {
                return CreateRateLimitedResponse("shared", isGlobal: false, bodyRetryAfter: 1, headerRetryAfter: 1);
            }
            case RateLimitKind.Global:
            {
                return CreateRateLimitedResponse("global", isGlobal: true, bodyRetryAfter: 1, headerRetryAfter: 1);
            }
            default:
            {
                var response = new FakeHttpResponse
                {
                    StatusCode = HttpResponseStatusCode.TooManyRequests,
                    Body = Encoding.UTF8.GetBytes("<html>Too many requests.</html>")
                };

                response.Headers[RestApiHeaderNames.RetryAfter] = "1";
                return response;
            }
        }
    }

    private static FakeHttpResponse CreateRateLimitedResponse(string scope, bool isGlobal, double bodyRetryAfter, int headerRetryAfter, double? resetAfter = null)
    {
        var body = string.Format(CultureInfo.InvariantCulture,
            "{{\"message\":\"You are being rate limited.\",\"retry_after\":{0},\"global\":{1}}}",
            bodyRetryAfter,
            isGlobal
                ? "true"
                : "false");

        var response = new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.TooManyRequests,
            Body = Encoding.UTF8.GetBytes(body)
        };

        response.Headers["Via"] = "1.1 google";
        response.Headers[RestApiHeaderNames.RetryAfter] = headerRetryAfter.ToString(CultureInfo.InvariantCulture);
        response.Headers[RestApiHeaderNames.RateLimitScope] = scope;
        if (isGlobal)
        {
            response.Headers[RestApiHeaderNames.RateLimitGlobal] = "true";
        }

        if (resetAfter != null)
        {
            response.Headers[RestApiHeaderNames.RateLimitResetAfter] = resetAfter.Value.ToString(CultureInfo.InvariantCulture);
        }

        return response;
    }

    public enum TokenRouteKind
    {
        Webhook,
        Interaction
    }

    public enum RateLimitKind
    {
        User,
        Shared,
        Global,
        Cloudflare
    }

    private sealed class GatedTimeProvider(ManualTimeProvider timeProvider) : TimeProvider
    {
        public Task Blocked => _blocked.Task;

        private readonly TaskCompletionSource _blocked = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _released = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _isArmed;

        /// <summary>
        ///     Makes the next <see cref="GetUtcNow"/> call block until <see cref="Release"/> is called.
        /// </summary>
        public void Arm()
        {
            Volatile.Write(ref _isArmed, 1);
        }

        public void Release()
        {
            _released.SetResult();
        }

        public override DateTimeOffset GetUtcNow()
        {
            if (Interlocked.Exchange(ref _isArmed, 0) == 1)
            {
                _blocked.SetResult();
                _released.Task.Wait();
            }

            return timeProvider.GetUtcNow();
        }

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            return timeProvider.CreateTimer(callback, state, dueTime, period);
        }
    }

    private sealed class ExternalLock : IDisposable
    {
        private readonly object _lockedObject;
        private readonly ManualResetEventSlim _lockHeld = new();
        private readonly ManualResetEventSlim _lockReleased = new();
        private readonly Thread _lockingThread;

        public ExternalLock(object lockedObject)
        {
            _lockedObject = lockedObject;
            _lockingThread = new Thread(static state => ((ExternalLock) state!).HoldLock());
            _lockingThread.Start(this);
            _lockHeld.Wait();
        }

        public void Dispose()
        {
            _lockReleased.Set();
            _lockingThread.Join();
            _lockHeld.Dispose();
            _lockReleased.Dispose();
        }

        private void HoldLock()
        {
            lock (_lockedObject)
            {
                _lockHeld.Set();
                _lockReleased.Wait();
            }
        }
    }

    private sealed class RecordingLogger(LogLevel minimumLevel = LogLevel.Debug) : ILogger<DefaultRestRateLimiter>
    {
        public IReadOnlyCollection<(LogLevel Level, string Message)> Entries => _entries.ToArray();

        private readonly ConcurrentQueue<(LogLevel Level, string Message)> _entries = new();

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= minimumLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                _entries.Enqueue((logLevel, formatter(state, exception)));
            }
        }
    }

    private sealed class ScriptedRestRequester : IRestRequester
    {
        public IRestApiClient ApiClient => _binder.Value;

        public ILogger Logger => NullLogger.Instance;

        public int Version => 10;

        public IHttpClient HttpClient => throw new NotSupportedException();

        private readonly ConcurrentDictionary<string, ConcurrentQueue<ScriptedExchange>> _exchanges = new();
        private readonly Binder<IRestApiClient> _binder;

        public ScriptedRestRequester()
        {
            _binder = new Binder<IRestApiClient>(this);
        }

        public void Bind(IRestApiClient value)
        {
            _binder.Bind(value);
        }

        public ScriptedExchange Script(IFormattedRoute route)
        {
            var exchange = new ScriptedExchange();
            _exchanges.GetOrAdd(route.Path, static _ => new ConcurrentQueue<ScriptedExchange>()).Enqueue(exchange);
            return exchange;
        }

        public Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default)
        {
            if (!_exchanges.TryGetValue(request.Route.Path, out var exchanges) || !exchanges.TryDequeue(out var exchange))
            {
                throw new InvalidOperationException($"No scripted exchange for {request.Route}.");
            }

            return exchange.Send();
        }

        public Task<IHttpResponse> ExecuteAuxiliaryAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class ScriptedExchange
    {
        public Task Sent => _sent.Task;

        private readonly TaskCompletionSource _sent = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource<IRestResponse> _response = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task<IRestResponse> Send()
        {
            _sent.SetResult();
            return _response.Task;
        }

        public void Respond(IHttpResponse httpResponse)
        {
            _response.SetResult(new DefaultRestResponse(httpResponse));
        }

        public void Fail(Exception exception)
        {
            _response.SetException(exception);
        }
    }
}

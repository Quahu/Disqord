using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Default;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Disqord.Tests.Unit.Gateway;

public class DefaultGatewayRateLimiterTests
{
    [Test]
    public async Task WaitAsync_MasterBucketWaitCancelledAfterOperationBucketAcquired_ReleasesTheOperationBucketSlot()
    {
        // Arrange
        var rateLimiter = new DefaultGatewayRateLimiter(
            Options.Create(new DefaultGatewayRateLimiterConfiguration()),
            NullLoggerFactory.Instance);
        rateLimiter.Bind(new FakeShard());

        var masterCount = rateLimiter.GetRemainingRequests();
        for (var i = 0; i < masterCount; i++)
        {
            await rateLimiter.WaitAsync(null);
        }

        var presenceBefore = rateLimiter.GetRemainingRequests(GatewayPayloadOperation.UpdatePresence);

        using var cancellationTokenSource = new CancellationTokenSource();
        var waitTask = rateLimiter.WaitAsync(GatewayPayloadOperation.UpdatePresence, cancellationTokenSource.Token);

        // Act
        await cancellationTokenSource.CancelAsync();

        // Assert
        Assert.That(async () => await waitTask, Throws.InstanceOf<OperationCanceledException>());
        Assert.That(rateLimiter.GetRemainingRequests(GatewayPayloadOperation.UpdatePresence), Is.EqualTo(presenceBefore));
    }

    private sealed class FakeShard : IShard
    {
        public ShardId Id => throw new NotImplementedException();
        public GatewayIntents Intents => throw new NotImplementedException();
        public GatewayCapabilities Capabilities => throw new NotImplementedException();
        public int LargeGuildThreshold => throw new NotImplementedException();
        public UpdatePresenceJsonModel? Presence { get; set; }
        public IGatewayApiClient ApiClient => throw new NotImplementedException();
        public IJsonSerializer Serializer => throw new NotImplementedException();
        public IGateway Gateway => throw new NotImplementedException();
        public IGatewayRateLimiter RateLimiter => throw new NotImplementedException();
        public IGatewayHeartbeater Heartbeater => throw new NotImplementedException();
        public string? SessionId => throw new NotImplementedException();
        public int? Sequence => throw new NotImplementedException();
        public Uri? ResumeUri => throw new NotImplementedException();
        public ShardState State => throw new NotImplementedException();
        public CancellationToken StoppingToken => throw new NotImplementedException();
        public ILogger Logger => NullLogger.Instance;
        public Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task WaitForReadyAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task RunAsync(Uri? initialUri, CancellationToken stoppingToken) => throw new NotImplementedException();
        public ValueTask DisposeAsync() => throw new NotImplementedException();
    }
}

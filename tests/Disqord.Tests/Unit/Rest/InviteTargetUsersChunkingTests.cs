using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Unit.Rest;

public class InviteTargetUsersChunkingTests
{
    private const string InviteCode = "code";

    private static readonly Snowflake[] UserIds = Enumerable.Range(1, 2500)
        .Select(static id => new Snowflake((ulong) id))
        .ToArray();

    private static readonly int[] ExpectedChunkSizes =
    [
        1000,
        1000,
        500
    ];

    [Test]
    public async Task AddInviteTargetUsersAsync_MoreThanMaxAmount_SplitsIntoMaxAmountChunks()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        await client.AddInviteTargetUsersAsync(InviteCode, UserIds);

        // Assert
        Assert.That(apiClient.Requests.Select(static request => request.Route), Has.All.SameAs(Route.Invite.AddTargetUsers));
        Assert.That(apiClient.Requests.Select(static request => request.UserIds.Length), Is.EqualTo(ExpectedChunkSizes));
        Assert.That(apiClient.Requests.SelectMany(static request => request.UserIds), Is.EqualTo(UserIds));
    }

    [Test]
    public async Task RemoveInviteTargetUsersAsync_MoreThanMaxAmount_SplitsIntoMaxAmountChunks()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        await client.RemoveInviteTargetUsersAsync(InviteCode, UserIds);

        // Assert
        Assert.That(apiClient.Requests.Select(static request => request.Route), Has.All.SameAs(Route.Invite.RemoveTargetUsers));
        Assert.That(apiClient.Requests.Select(static request => request.UserIds.Length), Is.EqualTo(ExpectedChunkSizes));
        Assert.That(apiClient.Requests.SelectMany(static request => request.UserIds), Is.EqualTo(UserIds));
    }

    [Test]
    public async Task AddInviteTargetUsersAsync_NoUserIds_DoesNotCallApi()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        await client.AddInviteTargetUsersAsync(InviteCode, Array.Empty<Snowflake>());

        // Assert
        Assert.That(apiClient.Requests, Is.Empty);
    }

    [Test]
    public async Task RemoveInviteTargetUsersAsync_NoUserIds_DoesNotCallApi()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        await client.RemoveInviteTargetUsersAsync(InviteCode, Array.Empty<Snowflake>());

        // Assert
        Assert.That(apiClient.Requests, Is.Empty);
    }

    [Test]
    public async Task EnumerateInviteTargetUserAddition_MoreThanMaxAmount_YieldsUserIdsOfEachChunk()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        var pages = new List<IReadOnlyList<Snowflake>>();
        await foreach (var page in client.EnumerateInviteTargetUserAddition(InviteCode, UserIds))
        {
            pages.Add(page);
        }

        // Assert
        Assert.That(pages.Select(static page => page.Count), Is.EqualTo(ExpectedChunkSizes));
        Assert.That(pages.SelectMany(static page => page), Is.EqualTo(UserIds));
        Assert.That(apiClient.Requests.Select(static request => request.Route), Has.All.SameAs(Route.Invite.AddTargetUsers));
    }

    [Test]
    public async Task EnumerateInviteTargetUserRemoval_MoreThanMaxAmount_YieldsUserIdsOfEachChunk()
    {
        // Arrange
        var apiClient = new RecordingInviteTargetUsersApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        var pages = new List<IReadOnlyList<Snowflake>>();
        await foreach (var page in client.EnumerateInviteTargetUserRemoval(InviteCode, UserIds))
        {
            pages.Add(page);
        }

        // Assert
        Assert.That(pages.Select(static page => page.Count), Is.EqualTo(ExpectedChunkSizes));
        Assert.That(pages.SelectMany(static page => page), Is.EqualTo(UserIds));
        Assert.That(apiClient.Requests.Select(static request => request.Route), Has.All.SameAs(Route.Invite.RemoveTargetUsers));
    }

    private sealed class RecordingInviteTargetUsersApiClient : IRestApiClient
    {
        public List<(IFormattableRoute Route, Snowflake[] UserIds)> Requests { get; } = new();

        public Token Token => throw new NotSupportedException();

        public ILogger Logger => throw new NotSupportedException();

        public IRestRateLimiter RateLimiter => throw new NotSupportedException();

        public IRestRequester Requester => throw new NotSupportedException();

        public IJsonSerializer Serializer => throw new NotSupportedException();

        public Task ExecuteAsync(IFormattedRoute route, IRestRequestContent? content = null,
            IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        {
            var targetUsersContent = (InviteTargetUsersJsonRestRequestContent) content!;
            Requests.Add((route.BaseRoute, targetUsersContent.UserIds.ToArray()));
            return Task.CompletedTask;
        }

        public Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route, IRestRequestContent? content = null,
            IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
            where TModel : class
        {
            throw new NotSupportedException();
        }
    }
}

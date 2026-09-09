using Disqord.Http;
using Disqord.Models;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;
using Disqord.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Tests.Unit.Rest;

public class BulkBanChunkingTests
{
    private static readonly Snowflake GuildId = new(1234UL);

    [Test]
    public async Task CreateBansAsync_MoreThanPageSize_SplitsIntoPageSizedChunks()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 450).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(apiClient.Chunks.Select(static chunk => chunk.Length), Is.EqualTo(new[]
        {
            200,
            200,
            50
        }));

        Assert.That(apiClient.Chunks.SelectMany(static chunk => chunk), Is.EqualTo(userIds));
        Assert.That(response.BannedUserIds, Is.EqualTo(userIds));
        Assert.That(response.FailedUserIds, Is.Empty);
    }

    [Test]
    public async Task CreateBansAsync_MoreThanPageSize_AggregatesBannedAndFailedAcrossChunks()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static chunk => new GuildBulkBanJsonModel
            {
                BannedUsers = chunk.Where(static id => id.RawValue % 2 == 0).ToArray(),
                FailedUsers = chunk.Where(static id => id.RawValue % 2 != 0).ToArray()
            }
        };

        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 450).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(apiClient.Chunks.Select(static chunk => chunk.Length), Is.EqualTo(new[]
        {
            200,
            200,
            50
        }));

        Assert.That(response.BannedUserIds, Is.EqualTo(userIds.Where(static id => id.RawValue % 2 == 0)));
        Assert.That(response.FailedUserIds, Is.EqualTo(userIds.Where(static id => id.RawValue % 2 != 0)));
    }

    [Test]
    public async Task CreateBansAsync_BatchFailsButOthersSucceed_FoldsFailedIdsAndDoesNotThrow()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static chunk => chunk.Any(static id => id.RawValue == 250)
                ? throw FailedToBanUsersException()
                : new GuildBulkBanJsonModel
                {
                    BannedUsers = chunk,
                    FailedUsers = []
                }
        };

        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 450).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        var bannedUserIds = Enumerable.Range(1, 200).Concat(Enumerable.Range(401, 50)).Select(static i => new Snowflake((ulong) i));
        var failedUserIds = Enumerable.Range(201, 200).Select(static i => new Snowflake((ulong) i));
        Assert.That(response.BannedUserIds, Is.EqualTo(bannedUserIds));
        Assert.That(response.FailedUserIds, Is.EqualTo(failedUserIds));
    }

    [Test]
    public async Task CreateBansAsync_AllBatchesFail_ReturnsAllUsersAsFailedAndDoesNotThrow()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static _ => throw FailedToBanUsersException()
        };

        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 450).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(response.BannedUserIds, Is.Empty);
        Assert.That(response.FailedUserIds, Is.EqualTo(userIds));
    }

    [Test]
    public async Task CreateBansAsync_SingleBatchAllFail_ReturnsAllUsersAsFailedAndDoesNotThrow()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static _ => throw FailedToBanUsersException()
        };

        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 50).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(response.BannedUserIds, Is.Empty);
        Assert.That(response.FailedUserIds, Is.EqualTo(userIds));
    }

    [Test]
    public void CreateBansAsync_ApiClientVariant_StillThrowsFailedToBanUsers()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static _ => throw FailedToBanUsersException()
        };

        var content = new CreateBansJsonRestRequestContent
        {
            UserIds = Enumerable.Range(1, 50).Select(static i => new Snowflake((ulong) i)).ToArray()
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<RestApiException>(async () => await apiClient.CreateBansAsync(GuildId, content));
        Assert.That(exception!.IsError(RestApiErrorCode.FailedToBanUsers), Is.True);
    }

    [Test]
    public async Task CreateBansAsync_AtMostPageSize_SendsSingleRequest()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, Discord.Limits.Rest.BulkBanUsersPageSize).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(apiClient.Chunks, Has.Count.EqualTo(1));
        Assert.That(apiClient.Chunks[0].Length, Is.EqualTo(Discord.Limits.Rest.BulkBanUsersPageSize));
        Assert.That(response.BannedUserIds, Is.EqualTo(userIds));
    }

    [Test]
    public async Task CreateBansAsync_NoUserIds_DoesNotCallApiAndReturnsEmptyResponse()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);

        // Act
        var response = await client.CreateBansAsync(GuildId, Array.Empty<Snowflake>());

        // Assert
        Assert.That(apiClient.Chunks, Is.Empty);
        Assert.That(response.BannedUserIds, Is.Empty);
        Assert.That(response.FailedUserIds, Is.Empty);
    }

    [Test]
    public async Task CreateBansAsync_WithDeleteMessageDuration_PassesDeleteMessageSecondsOnEveryChunk()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 250).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        await client.CreateBansAsync(GuildId, userIds, TimeSpan.FromDays(1));

        // Assert
        Assert.That(apiClient.DeleteMessageSeconds, Is.EqualTo(new int?[]
        {
            86400,
            86400
        }));
    }

    [Test]
    public async Task CreateBansAsync_WithDuplicateUserIds_DeduplicatesPreservingOrder()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = new[]
        {
            3UL,
            1UL,
            2UL,
            1UL,
            3UL
        }.Select(static i => new Snowflake(i)).ToArray();

        // Act
        var response = await client.CreateBansAsync(GuildId, userIds);

        // Assert
        var uniqueUserIds = new[]
        {
            3UL,
            1UL,
            2UL
        }.Select(static i => new Snowflake(i));
        Assert.That(apiClient.Chunks, Has.Count.EqualTo(1));
        Assert.That(apiClient.Chunks[0], Is.EqualTo(uniqueUserIds));
        Assert.That(response.BannedUserIds, Is.EqualTo(uniqueUserIds));
    }

    [Test]
    public async Task CreateBansAsync_DuplicatesSpanningPageSize_ChunksOnUniqueCount()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var uniqueUserIds = Enumerable.Range(1, Discord.Limits.Rest.BulkBanUsersPageSize).Select(static i => new Snowflake((ulong) i)).ToArray();
        var userIds = uniqueUserIds.Concat(uniqueUserIds).ToArray();

        // Act
        await client.CreateBansAsync(GuildId, userIds);

        // Assert
        Assert.That(apiClient.Chunks, Has.Count.EqualTo(1));
        Assert.That(apiClient.Chunks[0], Is.EqualTo(uniqueUserIds));
    }

    [Test]
    public async Task EnumerateBanCreation_YieldsOneResponsePerBatch()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 450).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var pages = new List<IReadOnlyList<IBulkBanResponse>>();
        await foreach (var page in client.EnumerateBanCreation(GuildId, userIds))
            pages.Add(page);

        // Assert
        Assert.That(pages.Select(static page => page.Count), Is.EqualTo(new[]
        {
            1,
            1,
            1
        }));

        Assert.That(pages.SelectMany(static page => page).SelectMany(static response => response.BannedUserIds), Is.EqualTo(userIds));
        Assert.That(apiClient.Chunks.Select(static chunk => chunk.Length), Is.EqualTo(new[]
        {
            200,
            200,
            50
        }));
    }

    [Test]
    public async Task EnumerateBanCreation_BatchFails_YieldsResponseWithFailedUsersAndDoesNotThrow()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient
        {
            RespondWith = static _ => throw FailedToBanUsersException()
        };

        var client = new FakeRestClient(apiClient);
        var userIds = Enumerable.Range(1, 50).Select(static i => new Snowflake((ulong) i)).ToArray();

        // Act
        var responses = new List<IBulkBanResponse>();
        await foreach (var page in client.EnumerateBanCreation(GuildId, userIds))
            responses.AddRange(page);

        // Assert
        Assert.That(responses, Has.Count.EqualTo(1));
        Assert.That(responses[0].BannedUserIds, Is.Empty);
        Assert.That(responses[0].FailedUserIds, Is.EqualTo(userIds));
    }

    [Test]
    public async Task EnumerateBanCreation_WithDuplicateUserIds_Deduplicates()
    {
        // Arrange
        var apiClient = new RecordingBulkBanApiClient();
        var client = new FakeRestClient(apiClient);
        var userIds = new[]
        {
            5UL,
            5UL,
            7UL,
            5UL,
            9UL
        }.Select(static i => new Snowflake(i)).ToArray();

        // Act
        await foreach (var _ in client.EnumerateBanCreation(GuildId, userIds))
        { }

        // Assert
        var uniqueUserIds = new[]
        {
            5UL,
            7UL,
            9UL
        }.Select(static i => new Snowflake(i));
        Assert.That(apiClient.Chunks, Has.Count.EqualTo(1));
        Assert.That(apiClient.Chunks[0], Is.EqualTo(uniqueUserIds));
    }

    private static RestApiException FailedToBanUsersException()
    {
        var errorModel = new RestApiErrorJsonModel
        {
            Code = RestApiErrorCode.FailedToBanUsers,
            Message = "Failed to ban users"
        };

        var httpResponse = new FakeHttpResponse
        {
            StatusCode = HttpResponseStatusCode.BadRequest,
            ReasonPhrase = "Bad Request"
        };

        return new RestApiException(httpResponse, "Bad Request", errorModel);
    }

    private sealed class RecordingBulkBanApiClient : IRestApiClient
    {
        public List<Snowflake[]> Chunks { get; } = new();

        public List<int?> DeleteMessageSeconds { get; } = new();

        public Func<Snowflake[], GuildBulkBanJsonModel> RespondWith { get; set; }
            = static chunk => new GuildBulkBanJsonModel
            {
                BannedUsers = chunk,
                FailedUsers = []
            };

        public Token Token => throw new NotSupportedException();

        public ILogger Logger => throw new NotSupportedException();

        public IRestRateLimiter RateLimiter => throw new NotSupportedException();

        public IRestRequester Requester => throw new NotSupportedException();

        public IJsonSerializer Serializer => throw new NotSupportedException();

        public Task ExecuteAsync(IFormattedRoute route, IRestRequestContent? content = null,
            IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route, IRestRequestContent? content = null,
            IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
            where TModel : class
        {
            var banContent = (CreateBansJsonRestRequestContent) content!;
            var chunk = banContent.UserIds.ToArray();
            Chunks.Add(chunk);
            DeleteMessageSeconds.Add(banContent.DeleteMessageSeconds.GetValueOrNullable());

            var model = RespondWith(chunk);
            return Task.FromResult((TModel) (object) model);
        }
    }
}

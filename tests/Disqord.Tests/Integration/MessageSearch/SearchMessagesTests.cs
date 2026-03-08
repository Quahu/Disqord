using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Tests.Integration.MessageSearch;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
[NonParallelizable]
[Order(1)]
public class SearchMessagesTests : IntegrationTestBase
{
    private static readonly Snowflake TestChannelId = 1477842759569113158;

    private readonly List<Snowflake> _sentMessageIds = [];

    private const int ExpectedSeedCount = 3;
    private const int ThrottleMs = 1_000;

    private static readonly string[] SeedMessages =
    [
        "[SearchTest] Alpha Bravo Charlie",
        "[SearchTest] Delta Echo Foxtrot",
        "[SearchTest] Alpha Delta Golf"
    ];

    [OneTimeSetUp]
    public async Task SeedTestMessages()
    {
        var existing = await RestClient.SearchMessagesAsync(IntegrationGuildId,
            new LocalMessageSearch
            {
                Contents = new List<string> { "SearchTest" },
                ChannelIds = new List<Snowflake> { TestChannelId }
            },
            waitUntilIndexReady: true);

        if (existing.FoundMessages.Count >= ExpectedSeedCount)
            return;

        foreach (var content in SeedMessages)
        {
            var msg = await RestClient.SendMessageAsync(TestChannelId, new LocalMessage { Content = content });
            _sentMessageIds.Add(msg.Id);
            await Task.Delay(ThrottleMs);
        }

        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        for (var attempt = 0; attempt < 20; attempt++)
        {
            await Task.Delay(5_000);
            var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);
            if (result.FoundMessages.Count >= ExpectedSeedCount)
                return;
        }

        Assert.Fail("Seeded messages were not indexed within the timeout.");
    }

    [OneTimeTearDown]
    public async Task CleanupTestMessages()
    {
        foreach (var id in _sentMessageIds)
        {
            try
            {
                await RestClient.DeleteMessageAsync(TestChannelId, id);
                await Task.Delay(ThrottleMs);
            }
            catch
            { }
        }
    }

    /// <summary>
    ///     Retries a search until at least <paramref name="minResults"/> messages are returned,
    ///     working around Discord's eventually-consistent search index.
    /// </summary>
    private async Task<IMessageSearchResponse> SearchWithMinResultsAsync(
        LocalMessageSearch search, int minResults, int limit = Discord.Limits.Rest.SearchMessagesPageSize,
        MessageSearchSortMode sortBy = MessageSearchSortMode.Timestamp,
        MessageSearchSortOrder sortOrder = MessageSearchSortOrder.Descending)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            await Task.Delay(ThrottleMs);
            var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, limit: limit,
                sortBy: sortBy, sortOrder: sortOrder, waitUntilIndexReady: true);
            if (result.FoundMessages.Count >= minResults)
                return result;

            await Task.Delay(2_000);
        }

        Assert.Fail($"Search did not return at least {minResults} results after retries.");
        return null!; // unreachable
    }

    private static IReadOnlyList<IMessageSearchFoundMessage> GetFoundMessages(IMessageSearchResponse response)
        => response.FoundMessages as IReadOnlyList<IMessageSearchFoundMessage> ?? response.FoundMessages.ToList();

    private static IReadOnlyList<IMessage> GetMessages(IMessageSearchResponse response)
        => GetFoundMessages(response).Select(foundMessage => foundMessage.Message).ToList();

    [Test]
    [Retry(2)]
    public async Task SearchMessages_EmptySearch_ReturnsResults()
    {
        // Arrange
        var search = new LocalMessageSearch();

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);

        // Assert
        Assert.That(result.TotalResultCount, Is.GreaterThan(0));
        Assert.That(result.FoundMessages, Is.Not.Empty);
        Assert.That(result.FoundMessages.All(foundMessage => foundMessage.MessageWithContext.Count > 0), Is.True);
        Assert.That(result.Threads, Is.Not.Null);
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_ContentFilter_FindsMatches()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, ExpectedSeedCount);

        // Assert
        Assert.That(result.TotalResultCount, Is.GreaterThanOrEqualTo(ExpectedSeedCount));
        Assert.That(result.FoundMessages.Count, Is.GreaterThanOrEqualTo(ExpectedSeedCount));

        foreach (var foundMessage in result.FoundMessages)
        {
            Assert.That(foundMessage.Message.Content, Does.Contain("SearchTest"));
        }
    }

    [Test]
    public async Task SearchMessages_ContentFilter_NoMatches_ReturnsEmpty()
    {
        // Arrange
        await Task.Delay(ThrottleMs);
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "zzz_no_match_ever_xyz_12345" }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);

        // Assert
        Assert.That(result.TotalResultCount, Is.EqualTo(0));
        Assert.That(result.FoundMessages, Is.Empty);
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_ChannelFilter_RestrictsToChannel()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, ExpectedSeedCount);

        // Assert
        Assert.That(result.TotalResultCount, Is.GreaterThanOrEqualTo(ExpectedSeedCount));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_SortByTimestampDescending_ReturnsMostRecentFirst()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 2,
            sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Descending);

        // Assert
        var messages = GetMessages(result);
        for (var i = 1; i < messages.Count; i++)
        {
            Assert.That(messages[i - 1].Id.RawValue, Is.GreaterThan(messages[i].Id.RawValue));
        }
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_SortByTimestampAscending_ReturnsOldestFirst()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 2,
            sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Ascending);

        // Assert
        var messages = GetMessages(result);
        for (var i = 1; i < messages.Count; i++)
        {
            Assert.That(messages[i - 1].Id.RawValue, Is.LessThan(messages[i].Id.RawValue));
        }
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_LimitParameter_RespectsLimit()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 1, limit: 1);

        // Assert
        Assert.That(result.FoundMessages.Count, Is.EqualTo(1));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_AfterId_ExcludesOlderMessages()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var baseResult = await SearchWithMinResultsAsync(search, ExpectedSeedCount,
            sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Ascending);

        var baseMessages = GetMessages(baseResult);
        var oldestId = baseMessages[0].Id;
        var afterId = baseMessages[1].Id;
        await Task.Delay(ThrottleMs);

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId,
            new LocalMessageSearch
            {
                Contents = new List<string> { "SearchTest" },
                ChannelIds = new List<Snowflake> { TestChannelId }
            },
            afterId: afterId, waitUntilIndexReady: true);

        // Assert
        var returnedIds = result.FoundMessages.Select(foundMessage => foundMessage.Message.Id).ToList();
        Assert.That(returnedIds, Does.Not.Contain(oldestId));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_FoundMessages_IncludeTheirContext()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 1);

        // Assert
        foreach (var foundMessage in result.FoundMessages)
        {
            Assert.That(foundMessage.MessageWithContext, Is.Not.Empty);
            var contextualMessage = foundMessage.MessageWithContext.Single(message => message.Id == foundMessage.Message.Id);
            Assert.That(contextualMessage, Is.SameAs(foundMessage.Message));
        }
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_ResponseProperties_ArePopulated()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 1);

        // Assert
        Assert.That(result.IsDoingDeepHistoricalIndex, Is.False);
        Assert.That(result.TotalResultCount, Is.GreaterThan(0));
        Assert.That(result.Threads, Is.Not.Null);
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_BeforeId_ExcludesNewerMessages()
    {
        // Arrange
        var baseSearch = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var baseResult = await SearchWithMinResultsAsync(baseSearch, ExpectedSeedCount,
            sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Descending);

        var baseMessages = GetMessages(baseResult);
        var newestId = baseMessages[0].Id;
        var beforeId = baseMessages[1].Id;
        await Task.Delay(ThrottleMs);

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId,
            new LocalMessageSearch
            {
                Contents = new List<string> { "SearchTest" },
                ChannelIds = new List<Snowflake> { TestChannelId }
            },
            beforeId: beforeId, waitUntilIndexReady: true);

        // Assert
        var returnedIds = result.FoundMessages.Select(foundMessage => foundMessage.Message.Id).ToList();
        Assert.That(returnedIds, Does.Not.Contain(newestId));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessages_MultipleContents_MatchesAll()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "Alpha", "Delta" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await SearchWithMinResultsAsync(search, 1);

        // Assert
        foreach (var foundMessage in result.FoundMessages)
        {
            Assert.That(foundMessage.Message.Content, Does.Contain("Alpha").And.Contain("Delta"));
        }
    }

    [Test]
    public async Task SearchMessages_PinnedFilter_ReturnsOnlyPinned()
    {
        // Arrange
        await Task.Delay(ThrottleMs);
        var search = new LocalMessageSearch
        {
            IsPinned = true,
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);

        // Assert
        Assert.That(result.TotalResultCount, Is.EqualTo(0));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessagesAsync_ReturnsSearchResponse()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);

        // Assert
        Assert.That(result.FoundMessages, Is.Not.Empty);
        Assert.That(result.FoundMessages.Count, Is.GreaterThanOrEqualTo(ExpectedSeedCount));

        foreach (var foundMessage in result.FoundMessages)
        {
            Assert.That(foundMessage.Message.Content, Does.Contain("SearchTest"));
        }
    }

    [Test]
    public async Task SearchMessagesAsync_LimitZero_ReturnsEmpty()
    {
        // Arrange
        await Task.Delay(ThrottleMs);
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "SearchTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, limit: 0, waitUntilIndexReady: true);

        // Assert
        Assert.That(result.FoundMessages, Is.Empty);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Tests.Integration.MessageSearch;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
[Order(2)]
public class SearchMessagesPaginationTests : IntegrationTestBase
{
    private static readonly Snowflake TestChannelId = 1477844972987482122;

    private const int SeededMessageCount = 60;
    private const int PageSize = 25;
    private const int ThrottleMs = 500;

    [OneTimeSetUp]
    public async Task SeedPaginationMessages()
    {
        var existing = await RestClient.FetchMessagesAsync(TestChannelId, 100);
        if (existing.Count >= SeededMessageCount)
        {
            return;
        }

        var toSend = SeededMessageCount - existing.Count;
        for (var i = 0; i < toSend; i++)
        {
            await RestClient.SendMessageAsync(TestChannelId,
                new LocalMessage { Content = $"[PaginationTest] Message {existing.Count + i:D3}" });

            await Task.Delay(ThrottleMs);
        }

        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        for (var attempt = 0; attempt < 30; attempt++)
        {
            await Task.Delay(5_000);
            var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, waitUntilIndexReady: true);
            if (result.TotalResultCount >= SeededMessageCount)
            {
                return;
            }
        }

        Assert.Fail($"Only {existing.Count} messages in channel; seeded but not all indexed within timeout.");
    }

    private static async Task<List<IMessage>> CollectMessagesAsync(IAsyncEnumerable<IMessageSearchResponse> enumerable)
    {
        var messages = new List<IMessage>();
        await foreach (var page in enumerable)
        {
            messages.AddRange(page.FoundMessages.Select(foundMessage => foundMessage.Message));
        }

        return messages;
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_ReturnsAllMessages()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var messages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                waitUntilIndexReady: true));

        // Assert
        Assert.That(messages.Count, Is.EqualTo(SeededMessageCount));
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_LimitLessThanPageSize_ReturnsSinglePage()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var limit = 10;

        // Act
        var pages = new List<IMessageSearchResponse>();
        await foreach (var page in RestClient.EnumerateMessageSearches(IntegrationGuildId, search, limit,
            waitUntilIndexReady: true))
        {
            pages.Add(page);
        }

        // Assert
        Assert.That(pages, Has.Count.EqualTo(1));
        Assert.That(pages[0].FoundMessages, Has.Count.EqualTo(limit));
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_LimitExactlyPageSize_ReturnsSinglePage()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var pages = new List<IMessageSearchResponse>();
        await foreach (var page in RestClient.EnumerateMessageSearches(IntegrationGuildId, search, PageSize,
            waitUntilIndexReady: true))
        {
            pages.Add(page);
        }

        // Assert
        Assert.That(pages, Has.Count.EqualTo(1));
        Assert.That(pages[0].FoundMessages, Has.Count.EqualTo(PageSize));
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_LimitSpansMultiplePages_ReturnsCorrectPageCount()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var pages = new List<IMessageSearchResponse>();
        await foreach (var page in RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
            waitUntilIndexReady: true))
        {
            pages.Add(page);
        }

        // Assert
        var expectedFullPages = SeededMessageCount / PageSize;
        var expectedRemainder = SeededMessageCount % PageSize;
        var expectedPageCount = expectedFullPages + (expectedRemainder > 0 ? 1 : 0);
        Assert.That(pages, Has.Count.EqualTo(expectedPageCount));

        for (var i = 0; i < expectedFullPages; i++)
        {
            Assert.That(pages[i].FoundMessages, Has.Count.EqualTo(PageSize));
        }

        if (expectedRemainder > 0)
        {
            Assert.That(pages[^1].FoundMessages, Has.Count.EqualTo(expectedRemainder));
        }
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_NoDuplicatesAcrossPages()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var messages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                waitUntilIndexReady: true));

        // Assert
        var ids = messages.Select(m => m.Id).ToList();
        Assert.That(ids, Is.Unique);
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_LimitSmallerThanTotal_StopsEarly()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var limit = PageSize + 5;

        // Act
        var messages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, limit,
                waitUntilIndexReady: true));

        // Assert
        Assert.That(messages.Count, Is.EqualTo(limit));
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_WithSearchFilter_OnlyReturnsMatches()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "zzz_pagination_no_match_xyz" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var messages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, 100,
                waitUntilIndexReady: true));

        // Assert
        Assert.That(messages, Is.Empty);
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessagesAsync_AutoPaginates_WhenLimitExceedsPageSize()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var limit = PageSize + 10;

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, limit: limit,
            waitUntilIndexReady: true);

        // Assert
        Assert.That(result.FoundMessages.Count, Is.EqualTo(limit));
        Assert.That(result.FoundMessages.Select(foundMessage => foundMessage.Message.Id), Is.Unique);
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessagesAsync_SinglePage_WhenLimitWithinPageSize()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, limit: 10,
            waitUntilIndexReady: true);

        // Assert
        Assert.That(result.FoundMessages.Count, Is.EqualTo(10));
    }

    [Test]
    [Retry(2)]
    public async Task SearchMessagesAsync_ReusesMessageInstancesAcrossAggregateResponse()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var result = await RestClient.SearchMessagesAsync(IntegrationGuildId, search, limit: PageSize + 10,
            waitUntilIndexReady: true);

        // Assert
        var messagesById = new Dictionary<Snowflake, IMessage>();
        foreach (var foundMessage in result.FoundMessages)
        {
            if (messagesById.TryGetValue(foundMessage.Message.Id, out var existingMessage))
            {
                Assert.That(foundMessage.Message, Is.SameAs(existingMessage));
            }
            else
            {
                messagesById.Add(foundMessage.Message.Id, foundMessage.Message);
            }

            var contextualFoundMessage = foundMessage.MessageWithContext.Single(message => message.Id == foundMessage.Message.Id);
            Assert.That(contextualFoundMessage, Is.SameAs(foundMessage.Message));

            foreach (var contextMessage in foundMessage.MessageWithContext)
            {
                if (messagesById.TryGetValue(contextMessage.Id, out existingMessage))
                {
                    Assert.That(contextMessage, Is.SameAs(existingMessage));
                }
                else
                {
                    messagesById.Add(contextMessage.Id, contextMessage);
                }
            }
        }
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_WithAfterId_ExcludesOlderMessages()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var allMessages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Ascending,
                waitUntilIndexReady: true));

        var cutoffId = allMessages[SeededMessageCount / 2].Id;

        // Act
        var filtered = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                afterId: cutoffId, waitUntilIndexReady: true));

        // Assert
        Assert.That(filtered, Is.Not.Empty);
        Assert.That(filtered.Count, Is.LessThan(allMessages.Count));

        foreach (var msg in filtered)
        {
            Assert.That(msg.Id.RawValue, Is.GreaterThan(cutoffId.RawValue));
        }
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_WithBeforeId_ExcludesNewerMessages()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        var allMessages = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                sortBy: MessageSearchSortMode.Timestamp, sortOrder: MessageSearchSortOrder.Descending,
                waitUntilIndexReady: true));

        var cutoffId = allMessages[SeededMessageCount / 2].Id;

        // Act
        var filtered = await CollectMessagesAsync(
            RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
                beforeId: cutoffId, waitUntilIndexReady: true));

        // Assert
        Assert.That(filtered, Is.Not.Empty);
        Assert.That(filtered.Count, Is.LessThan(allMessages.Count));

        foreach (var msg in filtered)
        {
            Assert.That(msg.Id.RawValue, Is.LessThan(cutoffId.RawValue));
        }
    }

    [Test]
    [Retry(2)]
    public async Task EnumerateMessageSearches_PageResponses_HaveTotalResultCount()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "PaginationTest" },
            ChannelIds = new List<Snowflake> { TestChannelId }
        };

        // Act
        var pages = new List<IMessageSearchResponse>();
        await foreach (var page in RestClient.EnumerateMessageSearches(IntegrationGuildId, search, SeededMessageCount,
            waitUntilIndexReady: true))
        {
            pages.Add(page);
        }

        // Assert
        Assert.That(pages, Is.Not.Empty);
        foreach (var page in pages)
        {
            Assert.That(page.TotalResultCount, Is.GreaterThanOrEqualTo(SeededMessageCount));
        }
    }
}

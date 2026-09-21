using Disqord.Rest;

namespace Disqord.Tests.Integration.Users;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
public class FetchGuildsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task FetchGuildsAsync_FirstShard_ReturnsIntegrationGuild()
    {
        // Act
        var guilds = await RestClient.FetchGuildsAsync(shard: 0);

        // Assert
        Assert.That(guilds.Select(static guild => guild.Id), Does.Contain(IntegrationGuildId));
    }
}

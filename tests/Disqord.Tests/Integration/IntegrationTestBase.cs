using System;
using System.Threading.Tasks;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Integration;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
public abstract class IntegrationTestBase
{
    private const string DisqordBotTokenVariableName = "DISQORD_INTEGRATION_BOT_TOKEN";
    private const string DisqordBotGuildIdVariableName = "DISQORD_INTEGRATION_BOT_GUILD_ID";

    private ServiceProvider? _serviceProvider;

    protected IRestClient RestClient { get; private set; } = null!;

    protected Snowflake IntegrationGuildId { get; private set; }

    [OneTimeSetUp]
    public void SetUpClient()
    {
        var token = Environment.GetEnvironmentVariable(DisqordBotTokenVariableName);
        if (string.IsNullOrWhiteSpace(token))
        {
            Assert.Ignore($"{DisqordBotTokenVariableName} not set; skipping integration tests.");
        }

        var guildIdStr = Environment.GetEnvironmentVariable(DisqordBotGuildIdVariableName);
        if (!string.IsNullOrWhiteSpace(guildIdStr) && Snowflake.TryParse(guildIdStr, out var guildId))
        {
            IntegrationGuildId = guildId;
        }
        else
        {
            Assert.Ignore($"{DisqordBotGuildIdVariableName} not set or invalid; skipping integration tests.");
        }

        var services = new ServiceCollection();
        services.AddToken(Token.Bot(token));
        services.AddRestClient();
        services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Warning));

        _serviceProvider = services.BuildServiceProvider();
        RestClient = _serviceProvider.GetRequiredService<IRestClient>();
    }

    [OneTimeTearDown]
    public async Task TearDownClient()
    {
        if (_serviceProvider != null)
        {
            await _serviceProvider.DisposeAsync();
        }
    }
}

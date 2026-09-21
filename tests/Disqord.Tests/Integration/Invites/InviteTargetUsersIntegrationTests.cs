using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Integration.Invites;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
[NonParallelizable]
public class InviteTargetUsersIntegrationTests : IntegrationTestBase
{
    private const string SecondaryBotTokenVariableName = "DISQORD_INTEGRATION_BOT2_TOKEN";

    private static readonly Snowflake InviteChannelId = 1477842759569113158;

    private static readonly int InviteMaxAgeSeconds = (int) TimeSpan.FromDays(1).TotalSeconds;

    private Snowflake _ownerId;
    private Snowflake _botId;
    private Snowflake _secondaryBotId;
    private string? _inviteCode;

    [OneTimeSetUp]
    public async Task FetchUserIds()
    {
        var secondaryBotToken = Environment.GetEnvironmentVariable(SecondaryBotTokenVariableName);
        if (string.IsNullOrWhiteSpace(secondaryBotToken))
        {
            Assert.Ignore($"{SecondaryBotTokenVariableName} not set; skipping integration tests.");
        }

        var guild = await RestClient.FetchGuildAsync(IntegrationGuildId);
        _ownerId = guild!.OwnerId;

        var currentUser = await RestClient.FetchCurrentUserAsync();
        _botId = currentUser.Id;

        _secondaryBotId = await FetchSecondaryBotIdAsync(secondaryBotToken);
    }

    [SetUp]
    public async Task CreateInvite()
    {
        var content = new CreateChannelInviteJsonRestRequestContent
        {
            MaxAge = InviteMaxAgeSeconds,
            TargetUserIds = new[]
            {
                _ownerId
            }
        };

        var invite = await RestClient.ApiClient.CreateChannelInviteAsync(InviteChannelId, content);
        _inviteCode = invite.Code;
    }

    [TearDown]
    public async Task DeleteInvite()
    {
        if (_inviteCode == null)
        {
            return;
        }

        await RestClient.DeleteInviteAsync(_inviteCode);
        _inviteCode = null;
    }

    [Test]
    public async Task CreateChannelInviteAsync_TargetUserIds_SetsTargetUsers()
    {
        // Act
        var targetUsers = await RestClient.FetchInviteTargetUsersAsync(_inviteCode!);

        // Assert
        Assert.That(targetUsers, Is.EquivalentTo(new[]
        {
            _ownerId
        }));
    }

    [Test]
    public async Task AddInviteTargetUserAsync_NewUser_AddsTargetUser()
    {
        // Act
        await RestClient.AddInviteTargetUserAsync(_inviteCode!, _botId);

        // Assert
        var targetUsers = await RestClient.FetchInviteTargetUsersAsync(_inviteCode!);
        Assert.That(targetUsers, Is.EquivalentTo(new[]
        {
            _ownerId,
            _botId
        }));
    }

    [Test]
    public async Task RemoveInviteTargetUserAsync_ExistingUser_RemovesTargetUser()
    {
        // Arrange
        await RestClient.AddInviteTargetUserAsync(_inviteCode!, _botId);

        // Act
        await RestClient.RemoveInviteTargetUserAsync(_inviteCode!, _botId);

        // Assert
        var targetUsers = await RestClient.FetchInviteTargetUsersAsync(_inviteCode!);
        Assert.That(targetUsers, Is.EquivalentTo(new[]
        {
            _ownerId
        }));
    }

    [Test]
    public async Task AddInviteTargetUsersAsync_NewUsers_AddsTargetUsers()
    {
        // Act
        await RestClient.AddInviteTargetUsersAsync(_inviteCode!, new[]
        {
            _botId,
            _secondaryBotId
        });

        // Assert
        var targetUsers = await RestClient.FetchInviteTargetUsersAsync(_inviteCode!);
        Assert.That(targetUsers, Is.EquivalentTo(new[]
        {
            _ownerId,
            _botId,
            _secondaryBotId
        }));
    }

    [Test]
    public async Task RemoveInviteTargetUsersAsync_ExistingUsers_RemovesTargetUsers()
    {
        // Arrange
        var userIds = new[]
        {
            _botId,
            _secondaryBotId
        };

        await RestClient.AddInviteTargetUsersAsync(_inviteCode!, userIds);

        // Act
        await RestClient.RemoveInviteTargetUsersAsync(_inviteCode!, userIds);

        // Assert
        var targetUsers = await RestClient.FetchInviteTargetUsersAsync(_inviteCode!);
        Assert.That(targetUsers, Is.EquivalentTo(new[]
        {
            _ownerId
        }));
    }

    private static async Task<Snowflake> FetchSecondaryBotIdAsync(string token)
    {
        var services = new ServiceCollection();
        services.AddToken(Token.Bot(token));
        services.AddRestClient();
        services.AddLogging(static logging => logging.SetMinimumLevel(LogLevel.Warning));

        await using (var serviceProvider = services.BuildServiceProvider())
        {
            var restClient = serviceProvider.GetRequiredService<IRestClient>();
            var currentUser = await restClient.FetchCurrentUserAsync();
            return currentUser.Id;
        }
    }
}

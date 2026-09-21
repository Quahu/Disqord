using Disqord.Rest;
using Disqord.Rest.Api;

namespace Disqord.Tests.Integration.Invites;

[TestFixture]
[Category("Integration")]
[DiscordIntegrationTest]
[NonParallelizable]
public class CreateChannelInviteIntegrationTests : IntegrationTestBase
{
    private static readonly Snowflake VoiceChannelId = 1537921512982450400;

    private static readonly Snowflake EmbeddedApplicationId = 880218394199220334;

    private string? _inviteCode;

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
    public async Task CreateInviteAsync_TargetApplicationId_CreatesEmbeddedApplicationInvite()
    {
        // Act
        var invite = (IGuildInvite) await RestClient.CreateInviteAsync(VoiceChannelId, targetApplicationId: EmbeddedApplicationId);
        _inviteCode = invite.Code;

        // Assert
        Assert.That(invite.TargetType, Is.EqualTo(InviteTargetType.EmbeddedApplication));
        Assert.That(invite.TargetApplication?.Id, Is.EqualTo(EmbeddedApplicationId));
    }

    [Test]
    public async Task CreateInviteAsync_TargetStreamUserNotStreaming_ThrowsInvalidFormBodyForTargetUserId()
    {
        // Arrange
        var guild = await RestClient.FetchGuildAsync(IntegrationGuildId);
        var ownerId = guild!.OwnerId;

        // Act
        var exception = Assert.ThrowsAsync<RestApiException>(async () => await RestClient.CreateInviteAsync(VoiceChannelId, targetStreamUserId: ownerId));

        // Assert
        Assert.That(exception!.IsError(RestApiErrorCode.InvalidFormBody), Is.True);
        Assert.That(exception.Message, Does.Contain("target_user_id"));
    }
}

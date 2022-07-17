using Disqord.Models;
using Qommon;

namespace Disqord;

public static class LocalGuildWelcomeScreenChannelExtensions
{
    public static WelcomeScreenChannelJsonModel ToModel(this LocalGuildWelcomeScreenChannel welcomeScreenChannel)
    {
        OptionalGuard.HasValue(welcomeScreenChannel.ChannelId);
        OptionalGuard.HasValue(welcomeScreenChannel.Description);

        return new()
        {
            ChannelId = welcomeScreenChannel.ChannelId.Value,
            Description = welcomeScreenChannel.Description.Value,
            EmojiId = (welcomeScreenChannel.Emoji.GetValueOrDefault() as LocalCustomEmoji)?.Id.GetValueOrNullable(),
            EmojiName = welcomeScreenChannel.Emoji.GetValueOrDefault()?.Name.GetValueOrDefault()
        };
    }
}

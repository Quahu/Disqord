using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalGuildWelcomeScreenChannelExtensions
{
    public static TWelcomeScreenChannel WithChannelId<TWelcomeScreenChannel>(this TWelcomeScreenChannel welcomeScreenChannel, Snowflake channelId)
        where TWelcomeScreenChannel : LocalGuildWelcomeScreenChannel
    {
        welcomeScreenChannel.ChannelId = channelId;
        return welcomeScreenChannel;
    }

    public static TWelcomeScreenChannel WithDescription<TWelcomeScreenChannel>(this TWelcomeScreenChannel welcomeScreenChannel, string description)
        where TWelcomeScreenChannel : LocalGuildWelcomeScreenChannel
    {
        welcomeScreenChannel.Description = description;
        return welcomeScreenChannel;
    }

    public static TWelcomeScreenChannel WithEmoji<TWelcomeScreenChannel>(this TWelcomeScreenChannel welcomeScreenChannel, LocalEmoji? emoji)
        where TWelcomeScreenChannel : LocalGuildWelcomeScreenChannel
    {
        welcomeScreenChannel.Emoji = emoji;
        return welcomeScreenChannel;
    }
}

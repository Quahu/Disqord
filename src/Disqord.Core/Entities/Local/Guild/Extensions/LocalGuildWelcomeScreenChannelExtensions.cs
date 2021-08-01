using Disqord.Models;

namespace Disqord
{
    public static class LocalGuildWelcomeScreenChannelExtensions
    {
        public static WelcomeScreenChannelJsonModel ToModel(this LocalGuildWelcomeScreenChannel channel)
            => new()
            {
                ChannelId = channel.ChannelId,
                Description = channel.Description,
                EmojiId = (channel.Emoji as LocalCustomEmoji).Id,
                EmojiName = channel.Emoji.Name
            };
    }
}

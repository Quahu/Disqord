using Disqord.Models;

namespace Disqord
{
    public class TransientGuildWelcomeScreenChannel : TransientEntity<WelcomeScreenChannelJsonModel>, IGuildWelcomeScreenChannel
    {
        public Snowflake ChannelId => Model.ChannelId;

        public string Description => Model.Description;

        public Snowflake? EmojiId => Model.EmojiId;

        public string EmojiName => Model.EmojiName;

        public TransientGuildWelcomeScreenChannel(IClient client, WelcomeScreenChannelJsonModel model)
            : base(client, model)
        { }
    }
}
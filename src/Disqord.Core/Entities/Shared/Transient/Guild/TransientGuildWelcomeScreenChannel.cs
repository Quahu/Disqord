using Disqord.Models;

namespace Disqord
{
    public class TransientGuildWelcomeScreenChannel : TransientEntity<WelcomeScreenChannelJsonModel>, IGuildWelcomeScreenChannel
    {
        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public IEmoji Emoji => _emoji ??= Disqord.Emoji.Create(new EmojiJsonModel
        {
            Id = Model.EmojiId,
            Name = Model.EmojiName
        });
        private IEmoji _emoji;

        public TransientGuildWelcomeScreenChannel(IClient client, WelcomeScreenChannelJsonModel model)
            : base(client, model)
        { }
    }
}

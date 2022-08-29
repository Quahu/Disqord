using Disqord.Models;

namespace Disqord;

public class TransientGuildWelcomeScreenChannel : TransientClientEntity<WelcomeScreenChannelJsonModel>, IGuildWelcomeScreenChannel
{
    /// <inheritdoc/>
    public Snowflake ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public string Description => Model.Description;

    /// <inheritdoc/>
    public IEmoji? Emoji
    {
        get
        {
            if (Model.EmojiName == null)
                return null;

            return _emoji ??= TransientEmoji.Create(new EmojiJsonModel
            {
                Id = Model.EmojiId,
                Name = Model.EmojiName
            });
        }
    }
    private IEmoji? _emoji;

    public TransientGuildWelcomeScreenChannel(IClient client, WelcomeScreenChannelJsonModel model)
        : base(client, model)
    { }
}

using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IChannelTag"/>
public class TransientChannelTag : TransientEntity<ChannelTagJsonModel>, IChannelTag
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id.Value;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public bool IsModerated => Model.Moderated;

    /// <inheritdoc/>
    public IEmoji Emoji
    {
        get
        {
            if (_emoji != null)
                return _emoji;

            var emoji = Model.EmojiId != null
                ? new TransientCustomEmoji(Model.EmojiId.Value)
                : new TransientEmoji(Model.EmojiName!);

            return _emoji = emoji;
        }
    }

    private IEmoji? _emoji;

    public TransientChannelTag(ChannelTagJsonModel model)
        : base(model)
    { }
}

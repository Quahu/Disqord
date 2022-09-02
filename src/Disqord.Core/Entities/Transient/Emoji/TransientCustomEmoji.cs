using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientCustomEmoji : TransientEmoji, ICustomEmoji
{
    public Snowflake Id => Model.Id!.Value;

    public bool IsAnimated => Model.Animated.GetValueOrDefault();

    public string Tag => ToString();

    public TransientCustomEmoji(EmojiJsonModel model)
        : base(model)
    { }

    public TransientCustomEmoji(Snowflake id, string? name = null)
        : base(new EmojiJsonModel
        {
            Id = id,
            Name = name
        })
    { }

    public bool Equals(ICustomEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }
}

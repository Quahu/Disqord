using Disqord.Models;

namespace Disqord;

public class TransientEmoji : TransientEntity<EmojiJsonModel>, IEmoji
{
    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name!;

    public TransientEmoji(EmojiJsonModel model)
        : base(model)
    { }

    public TransientEmoji(string name)
        : base(new EmojiJsonModel
        {
            Name = name
        })
    { }

    public bool Equals(IEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    public override bool Equals(object? obj)
    {
        return obj is IEmoji other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Comparers.Emoji.GetHashCode(this);
    }

    public override string ToString()
    {
        return this.GetString();
    }

    public static IEmoji Create(EmojiJsonModel model)
    {
        if (model.Id.HasValue)
            return new TransientCustomEmoji(model);

        return new TransientEmoji(model);
    }
}

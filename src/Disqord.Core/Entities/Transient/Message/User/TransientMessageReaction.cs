using Disqord.Models;

namespace Disqord;

public class TransientMessageReaction : TransientEntity<ReactionJsonModel>, IMessageReaction
{
    /// <inheritdoc/>
    public IEmoji Emoji => _emoji ??= TransientEmoji.Create(Model.Emoji);

    private IEmoji? _emoji;

    /// <inheritdoc/>
    public int Count => Model.Count;

    /// <inheritdoc/>
    public bool HasOwnReaction => Model.Me;

    public TransientMessageReaction(ReactionJsonModel model)
        : base(model)
    { }

    public TransientMessageReaction(EmojiJsonModel emojiModel, int count, bool hasOwnReaction)
        : base(new ReactionJsonModel
        {
            Emoji = emojiModel,
            Count = count,
            Me = hasOwnReaction
        })
    { }

    public override string ToString()
    {
        return this.GetString();
    }
}

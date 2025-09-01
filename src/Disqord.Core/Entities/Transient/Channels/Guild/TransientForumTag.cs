using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IForumTag"/>
public class TransientForumTag(ForumTagJsonModel model)
    : TransientEntity<ForumTagJsonModel>(model), IForumTag
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id.Value;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public bool IsModerated => Model.Moderated;

    /// <inheritdoc/>
    public IEmoji? Emoji
    {
        get
        {
            if (Model.EmojiId == null && Model.EmojiName == null)
            {
                return null;
            }

            return field ??= Model.EmojiId != null
                ? new TransientCustomEmoji(Model.EmojiId.Value)
                : new TransientEmoji(Model.EmojiName!);
        }
    }
}

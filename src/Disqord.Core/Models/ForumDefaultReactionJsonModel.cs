using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ForumDefaultReactionJsonModel : JsonModel
{
    /// <summary>
    ///     <see cref="EmojiId"/> and <see cref="EmojiName"/> are mutually exclusive.
    /// </summary>
    [JsonProperty("emoji_id")]
    public Snowflake? EmojiId;

    /// <summary>
    ///     <see cref="EmojiId"/> and <see cref="EmojiName"/> are mutually exclusive.
    /// </summary>
    [JsonProperty("emoji_name")]
    public string? EmojiName;

    public static ForumDefaultReactionJsonModel FromEmoji(IEmoji emoji)
    {
        var model = new ForumDefaultReactionJsonModel();
        if (emoji is ICustomEmoji customEmoji)
        {
            model.EmojiId = customEmoji.Id;
        }
        else
        {
            Guard.IsNotNull(emoji.Name);

            model.EmojiName = emoji.Name;
        }

        return model;
    }
}

using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ForumTagJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Optional<Snowflake> Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("moderated")]
    public bool Moderated;

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
}

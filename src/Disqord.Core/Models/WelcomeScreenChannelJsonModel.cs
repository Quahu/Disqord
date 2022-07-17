using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class WelcomeScreenChannelJsonModel : JsonModel
{
    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("description")]
    public string Description = null!;

    [JsonProperty("emoji_id")]
    public Snowflake? EmojiId;

    [JsonProperty("emoji_name")]
    public string? EmojiName;

    /// <inheritdoc />
    protected override void OnValidate()
    {
        Guard.IsNotNull(Description);
        Guard.HasSizeBetweenOrEqualTo(Description, Discord.Limits.Guild.WelcomeScreen.Channel.MinDescriptionLength, Discord.Limits.Guild.WelcomeScreen.Channel.MaxDescriptionLength);
    }
}

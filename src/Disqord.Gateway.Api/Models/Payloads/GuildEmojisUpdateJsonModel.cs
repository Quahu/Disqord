using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildEmojisUpdateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("emojis")]
    public EmojiJsonModel[] Emojis = null!;
}
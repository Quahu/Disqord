using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ActivityJsonModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("type")]
    public ActivityType Type;

    [JsonProperty("url")]
    public Optional<string> Url;

    [JsonProperty("created_at")]
    public long CreatedAt;

    [JsonProperty("timestamps")]
    public Optional<ActivityTimestampsJsonModel> Timestamps;

    [JsonProperty("application_id")]
    public Optional<Snowflake> ApplicationId;

    [JsonProperty("details")]
    public Optional<string> Details;

    [JsonProperty("state")]
    public Optional<string> State;

    [JsonProperty("emoji")]
    public Optional<EmojiJsonModel> Emoji;

    [JsonProperty("party")]
    public Optional<ActivityPartyJsonModel> Party;

    [JsonProperty("assets")]
    public Optional<ActivityAssetsJsonModel> Assets;

    [JsonProperty("secrets")]
    public Optional<ActivitySecretsJsonModel> Secrets;

    [JsonProperty("instance")]
    public Optional<bool> Instance;

    [JsonProperty("sync_id")]
    public Optional<string> SyncId;

    [JsonProperty("session_id")]
    public Optional<string> SessionId;

    [JsonProperty("flags")]
    public Optional<ActivityFlags> Flags;

    [JsonProperty("id")]
    public Optional<string> Id;
}

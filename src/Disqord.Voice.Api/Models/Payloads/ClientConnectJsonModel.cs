using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Voice.Api.Models;

public class ClientConnectJsonModel : JsonModel
{
    [JsonProperty("user_id")]
    public Optional<Snowflake> UserId;

    [JsonProperty("audio_ssrc")]
    public Optional<uint> AudioSsrc;

    [JsonProperty("video_ssrc")]
    public Optional<uint> VideoSsrc;

    [JsonProperty("user_ids")]
    public Snowflake[]? UserIds;
}

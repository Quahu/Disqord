using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class VoiceStateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("channel_id")]
    public Snowflake? ChannelId;

    [JsonProperty("user_id")]
    public Snowflake UserId;

    [JsonProperty("member")]
    public Optional<IJsonObject> Member;

    [JsonProperty("session_id")]
    public string SessionId = null!;

    [JsonProperty("deaf")]
    public bool Deaf;

    [JsonProperty("mute")]
    public bool Mute;

    [JsonProperty("self_deaf")]
    public bool SelfDeaf;

    [JsonProperty("self_mute")]
    public bool SelfMute;

    [JsonProperty("self_stream")]
    public Optional<bool> SelfStream;

    [JsonProperty("self_video")]
    public bool SelfVideo;

    [JsonProperty("suppress")]
    public bool Suppress;

    [JsonProperty("request_to_speak_timestamp")]
    public DateTimeOffset? RequestToSpeakTimestamp;
}
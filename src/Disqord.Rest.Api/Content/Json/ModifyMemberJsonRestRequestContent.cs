using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyMemberJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("nick")]
    public Optional<string> Nick;

    [JsonProperty("roles")]
    public Optional<Snowflake[]> Roles;

    [JsonProperty("mute")]
    public Optional<bool> Mute;

    [JsonProperty("deaf")]
    public Optional<bool> Deaf;

    [JsonProperty("channel_id")]
    public Optional<Snowflake?> ChannelId;

    [JsonProperty("communication_disabled_until")]
    public Optional<DateTimeOffset?> CommunicationDisabledUntil;
}
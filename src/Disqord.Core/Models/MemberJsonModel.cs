using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

[JsonSkippedProperties("is_pending", "hoisted_role")]
public class MemberJsonModel : JsonModel
{
    [JsonProperty("user")]
    public Optional<UserJsonModel> User;

    [JsonProperty("nick")]
    public Optional<string?> Nick;

    [JsonProperty("roles")]
    public Snowflake[] Roles = null!;

    [JsonProperty("joined_at")]
    public Optional<DateTimeOffset> JoinedAt;

    [JsonProperty("premium_since")]
    public Optional<DateTimeOffset?> PremiumSince;

    [JsonProperty("deaf")]
    public Optional<bool> Deaf;

    [JsonProperty("mute")]
    public Optional<bool> Mute;

    [JsonProperty("pending")]
    public Optional<bool> Pending;

    [JsonProperty("permissions")]
    public Optional<Permissions> Permissions;

    [JsonProperty("avatar")]
    public Optional<string?> Avatar;

    [JsonProperty("communication_disabled_until")]
    public Optional<DateTimeOffset?> CommunicationDisabledUntil;
}

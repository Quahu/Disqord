using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationCommandInteractionDataResolvedJsonModel : JsonModel
{
    [JsonProperty("users")]
    public Optional<Dictionary<Snowflake, UserJsonModel>> Users;

    [JsonProperty("members")]
    public Optional<Dictionary<Snowflake, MemberJsonModel>> Members;

    [JsonProperty("roles")]
    public Optional<Dictionary<Snowflake, RoleJsonModel>> Roles;

    [JsonProperty("channels")]
    public Optional<Dictionary<Snowflake, ChannelJsonModel>> Channels;

    [JsonProperty("messages")]
    public Optional<Dictionary<Snowflake, MessageJsonModel>> Messages;

    [JsonProperty("attachments")]
    public Optional<Dictionary<Snowflake, AttachmentJsonModel>> Attachments;
}

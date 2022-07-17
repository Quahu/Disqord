using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AllowedMentionsJsonModel : JsonModel
{
    [JsonProperty("parse")]
    public Optional<IList<string>> Parse;

    [JsonProperty("users")]
    public Optional<Snowflake[]> Users;

    [JsonProperty("roles")]
    public Optional<Snowflake[]> Roles;

    [JsonProperty("replied_user")]
    public Optional<bool> RepliedUser;
}

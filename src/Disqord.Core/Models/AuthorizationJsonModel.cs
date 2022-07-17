using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AuthorizationJsonModel : JsonModel
{
    [JsonProperty("application")]
    public ApplicationJsonModel Application = null!;

    [JsonProperty("scopes")]
    public string[] Scopes = null!;

    [JsonProperty("expires")]
    public DateTimeOffset Expires;

    [JsonProperty("user")]
    public Optional<UserJsonModel> User;
}

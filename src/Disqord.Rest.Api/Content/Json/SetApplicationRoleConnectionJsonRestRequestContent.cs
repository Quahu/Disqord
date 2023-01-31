using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class SetApplicationRoleConnectionJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("platform_name")]
    public Optional<string> PlatformName;

    [JsonProperty("platform_username")]
    public Optional<string> PlatformUsername;

    [JsonProperty("metadata")]
    public Optional<Dictionary<string, string>> Metadata;
}

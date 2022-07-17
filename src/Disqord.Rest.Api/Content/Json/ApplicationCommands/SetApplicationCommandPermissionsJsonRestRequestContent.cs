using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class SetApplicationCommandPermissionsJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("permissions")]
    public ApplicationCommandPermissionsJsonModel[] Permissions = null!;
}

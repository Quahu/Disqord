using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class InstallParamsJsonModel : JsonModel
    {
        [JsonProperty("scopes")]
        public string[] Scopes;

        [JsonProperty("permissions")]
        public ulong Permissions;
    }
}

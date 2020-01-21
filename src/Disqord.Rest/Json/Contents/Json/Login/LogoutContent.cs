using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class LogoutContent : JsonRequestContent
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("void_provider")]
        public string VoipProvider { get; set; }
    }
}

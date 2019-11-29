using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyCurrentUserContent : JsonRequestContent
    {
        [JsonProperty("username")]
        public Optional<string> Username { get; set; }

        [JsonProperty("avatar")]
        public Optional<Stream> Avatar { get; set; }

        [JsonProperty("discriminator")]
        public Optional<string> Discriminator { get; set; }

        [JsonProperty("new_password")]
        public Optional<string> NewPassword { get; set; }
    }
}

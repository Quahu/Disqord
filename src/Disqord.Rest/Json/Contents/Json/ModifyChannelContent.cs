using System.Collections.Generic;
using System.IO;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyChannelContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("icon")]
        public Optional<Stream> Icon { get; set; }

        [JsonProperty("position")]
        public Optional<int> Position { get; set; }

        [JsonProperty("topic")]
        public Optional<string> Topic { get; set; }

        [JsonProperty("nsfw")]
        public Optional<bool> Nsfw { get; set; }

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser { get; set; }

        [JsonProperty("bitrate")]
        public Optional<int> Bitrate { get; set; }

        [JsonProperty("user_limit")]
        public Optional<int> UserLimit { get; set; }

        [JsonProperty("permission_overwrites")]
        public Optional<IReadOnlyList<OverwriteModel>> PermissionOverwrites { get; set; }

        [JsonProperty("parent_id")]
        public Optional<ulong> ParentId { get; set; }
    }
}

using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateGuildChannelContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ChannelType Type { get; set; }

        [JsonProperty("topic")]
        public Optional<string> Topic { get; set; }

        [JsonProperty("bitrate")]
        public Optional<int> Bitrate { get; set; }

        [JsonProperty("user_limit")]
        public Optional<int> UserLimit { get; set; }

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser { get; set; }

        [JsonProperty("position")]
        public Optional<int> Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public Optional<IReadOnlyList<OverwriteModel>> PermissionOvewrites { get; set; }

        [JsonProperty("parent_id")]
        public Optional<ulong> ParentId { get; set; }

        [JsonProperty("nsfw")]
        public Optional<bool> Nsfw { get; set; }
    }
}

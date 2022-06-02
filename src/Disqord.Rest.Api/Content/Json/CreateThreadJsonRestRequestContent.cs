using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateThreadJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("auto_archive_duration")]
        public Optional<int> AutoArchiveDuration;

        [JsonProperty("type")]
        public Optional<ChannelType> Type;

        [JsonProperty("invitable")]
        public Optional<bool> Invitable;

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser;

        [JsonProperty("flags")]
        public Optional<ChannelFlag> Flags;
    }
}

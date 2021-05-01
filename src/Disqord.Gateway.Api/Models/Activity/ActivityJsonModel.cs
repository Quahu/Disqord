using System;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class ActivityJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public ActivityType Type;

        [JsonProperty("url")]
        public Optional<string> Url;

        public Optional<long> CreatedAt;

        public Optional<TimestampsJsonModel> Timestamps;
    }
}
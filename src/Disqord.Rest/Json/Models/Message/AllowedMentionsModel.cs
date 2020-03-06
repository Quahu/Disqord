using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AllowedMentionsModel
    {
        [JsonProperty("parse")]
        public IList<string> Parse { get; set; }

        [JsonProperty("users", NullValueHandling.Ignore)]
        public ulong[] Users { get; set; }

        [JsonProperty("roles", NullValueHandling.Ignore)]
        public ulong[] Roles { get; set; }
    }
}
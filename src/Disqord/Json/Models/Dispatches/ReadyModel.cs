using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class ReadyModel
    {
        [JsonProperty("v", NullValueHandling = NullValueHandling.Ignore)]
        public int V { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("private_channels", NullValueHandling = NullValueHandling.Ignore)]
        public ChannelModel[] PrivateChannels { get; set; }

        [JsonProperty("guilds", NullValueHandling = NullValueHandling.Ignore)]
        public WebSocketGuildModel[] Guilds { get; set; }

        [JsonProperty("notes")]
        public Dictionary<ulong, string> Notes { get; set; }

        [JsonProperty("relationships", NullValueHandling = NullValueHandling.Ignore)]
        public RelationshipModel[] Relationships { get; set; }

        [JsonProperty("session_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("_trace", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Trace { get; set; }
    }
}

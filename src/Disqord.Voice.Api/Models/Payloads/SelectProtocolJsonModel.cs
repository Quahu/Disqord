using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models
{
    public class SelectProtocolJsonModel : JsonModel
    {
        [JsonProperty("protocol")]
        public string Protocol;

        [JsonProperty("data")]
        public SelectProtocolDataJsonModel Data;

        public class SelectProtocolDataJsonModel : JsonModel
        {
            [JsonProperty("address")]
            public string Address;

            [JsonProperty("port")]
            public int Port;

            [JsonProperty("mode")]
            public string Mode;
        }
    }
}

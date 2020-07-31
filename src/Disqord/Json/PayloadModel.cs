using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class PayloadModel : JsonModel
    {
        /// <summary>
        ///     Gets or sets the opcode for the payload.
        /// </summary>
        [JsonProperty("op", NullValueHandling.Ignore)]
        public GatewayOperationCode Op { get; set; }

        /// <summary>
        ///     Gets or sets the event data.
        /// </summary>
        [JsonProperty("d", NullValueHandling.Ignore)]
        public IJsonElement D { get; set; }

        /// <summary>
        ///     Gets or sets the sequence number, used for resuming sessions and heartbeats.
        /// </summary>
        [JsonProperty("s", NullValueHandling.Ignore)]
        public int? S { get; set; }

        /// <summary>
        ///     Gets or sets the event name for this payload.
        /// </summary>
        [JsonProperty("T", NullValueHandling.Ignore)]
        public string T { get; set; }
    }
}
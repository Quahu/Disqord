using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class PayloadModel
    {
        /// <summary>
        ///     Gets or sets the opcode for the payload.
        /// </summary>
        [JsonProperty("op", NullValueHandling = NullValueHandling.Ignore)]
        public Opcode Op { get; set; }

        /// <summary>
        ///     Gets or sets the event data.
        /// </summary>
        [JsonProperty("d", NullValueHandling = NullValueHandling.Ignore)]
        public object D { get; set; }

        /// <summary>
        ///     Gets or sets the sequence number, used for resuming sessions and heartbeats.
        /// </summary>
        [JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
        public int? S { get; set; }

        /// <summary>
        ///     Gets or sets the event name for this payload.
        /// </summary>
        [JsonProperty("T", NullValueHandling = NullValueHandling.Ignore)]
        public object T { get; set; }
    }
}
using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class UserNoteUpdateModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }
    }
}

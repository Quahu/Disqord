using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateNoteContent : JsonRequestContent
    {
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}

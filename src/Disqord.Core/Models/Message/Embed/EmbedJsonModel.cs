using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class EmbedJsonModel : JsonModel
    {
        [JsonProperty("title")]
        public Optional<string> Title;

        [JsonProperty("type")]
        public Optional<string> Type;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("url")]
        public Optional<string> Url;

        [JsonProperty("timestamp")]
        public Optional<DateTimeOffset> Timestamp;

        [JsonProperty("color")]
        public Optional<int> Color;

        [JsonProperty("footer")]
        public Optional<EmbedFooterJsonModel> Footer;

        [JsonProperty("image")]
        public Optional<EmbedImageJsonModel> Image;

        [JsonProperty("thumbnail")]
        public Optional<EmbedThumbnailJsonModel> Thumbnail;

        [JsonProperty("video")]
        public Optional<EmbedVideoJsonModel> Video;

        [JsonProperty("provider")]
        public Optional<EmbedProviderJsonModel> Provider;

        [JsonProperty("author")]
        public Optional<EmbedAuthorJsonModel> Author;

        [JsonProperty("fields")]
        public Optional<EmbedFieldJsonModel[]> Fields;
    }
}

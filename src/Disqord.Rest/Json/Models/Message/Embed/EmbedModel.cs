using System;
using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        [JsonProperty("footer")]
        public EmbedFooterModel Footer { get; set; }

        [JsonProperty("image")]
        public EmbedImageModel Image { get; set; }

        [JsonProperty("thumbnail")]
        public EmbedThumbnailModel Thumbnail { get; set; }

        [JsonProperty("video")]
        public EmbedVideoModel Video { get; set; }

        [JsonProperty("provider")]
        public EmbedProviderModel Provider { get; set; }

        [JsonProperty("author")]
        public EmbedAuthorModel Author { get; set; }

        [JsonProperty("fields")]
        public IEnumerable<EmbedFieldModel> Fields { get; set; } = Array.Empty<EmbedFieldModel>();
    }
}

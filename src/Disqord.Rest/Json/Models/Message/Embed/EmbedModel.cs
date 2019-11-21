using System;
using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedModel
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public int? Color { get; set; }

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedFooterModel Footer { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedImageModel Image { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedThumbnailModel Thumbnail { get; set; }

        [JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedVideoModel Video { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedProviderModel Provider { get; set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public EmbedAuthorModel Author { get; set; }

        [JsonProperty("fields")]
        public IReadOnlyList<EmbedFieldModel> Fields { get; set; } = Array.Empty<EmbedFieldModel>();
    }
}

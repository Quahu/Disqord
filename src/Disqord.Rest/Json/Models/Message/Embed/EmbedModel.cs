using System;
using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedModel
    {
        [JsonProperty("title", NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("type", NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("timestamp", NullValueHandling.Ignore)]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("color", NullValueHandling.Ignore)]
        public int? Color { get; set; }

        [JsonProperty("footer", NullValueHandling.Ignore)]
        public EmbedFooterModel Footer { get; set; }

        [JsonProperty("image", NullValueHandling.Ignore)]
        public EmbedImageModel Image { get; set; }

        [JsonProperty("thumbnail", NullValueHandling.Ignore)]
        public EmbedThumbnailModel Thumbnail { get; set; }

        [JsonProperty("video", NullValueHandling.Ignore)]
        public EmbedVideoModel Video { get; set; }

        [JsonProperty("provider", NullValueHandling.Ignore)]
        public EmbedProviderModel Provider { get; set; }

        [JsonProperty("author", NullValueHandling.Ignore)]
        public EmbedAuthorModel Author { get; set; }

        [JsonProperty("fields")]
        public EmbedFieldModel[] Fields { get; set; } = Array.Empty<EmbedFieldModel>();
    }
}

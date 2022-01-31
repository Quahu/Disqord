using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ThreadMetadataJsonModel : JsonModel
    {
        [JsonProperty("archived")]
        public bool Archived;

        [JsonProperty("auto_archive_duration")]
        public int AutoArchiveDuration;

        [JsonProperty("archive_timestamp")]
        public DateTimeOffset ArchiveTimestamp;

        [JsonProperty("locked")]
        public Optional<bool> Locked;

        [JsonProperty("invitable")]
        public Optional<bool> Invitable;

        [JsonProperty("create_timestamp")]
        public Optional<DateTimeOffset?> CreateTimestamp;
    }
}

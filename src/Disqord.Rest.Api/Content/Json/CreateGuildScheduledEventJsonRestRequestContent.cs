using System;
using System.IO;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateGuildScheduledEventJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("entity_metadata")]
        public Optional<GuildScheduledEventEntityMetadataJsonModel> EntityMetadata;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("privacy_level")]
        public PrivacyLevel PrivacyLevel;

        [JsonProperty("scheduled_start_time")]
        public DateTimeOffset ScheduledStartTime;

        [JsonProperty("scheduled_end_time")]
        public Optional<DateTimeOffset> ScheduledEndTime;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("entity_type")]
        public GuildEventTargetType EntityType;

        [JsonProperty("image")]
        public Optional<Stream> Image;

        protected override void OnValidate()
        {
            switch (EntityType)
            {
                case GuildEventTargetType.Stage:
                case GuildEventTargetType.Voice:
                {
                    OptionalGuard.HasValue(ChannelId, "Stage or Voice events must have a channel ID set.");
                    OptionalGuard.HasNoValue(EntityMetadata, "Stage or Voice events must not have entity metadata set.");
                    break;
                }
                case GuildEventTargetType.External:
                {
                    OptionalGuard.HasNoValue(ChannelId, "External events must not have a channel ID set.");
                    OptionalGuard.CheckValue(EntityMetadata, metadata =>
                    {
                        Guard.IsNotNull(metadata);
                        ContentValidation.GuildEvents.Metadata.ValidateLocation(metadata.Location);
                    });
                    OptionalGuard.HasValue(ScheduledEndTime, "External events must have an end time set.");
                    break;
                }
            }

            ContentValidation.GuildEvents.ValidateName(Name);
            ContentValidation.GuildEvents.ValidateDescription(Description);
        }
    }
}

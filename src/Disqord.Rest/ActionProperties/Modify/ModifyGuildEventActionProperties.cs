using System;

namespace Disqord.Rest
{
    public class ModifyGuildEventActionProperties
    {
        public Optional<Snowflake> ChannelId;

        public Optional<string> Name;

        public Optional<StagePrivacyLevel> PrivacyLevel;

        public Optional<DateTimeOffset> ScheduledStartTime;

        public Optional<string> Description;

        public Optional<GuildScheduledEventEntityType> EntityType;
    }
}
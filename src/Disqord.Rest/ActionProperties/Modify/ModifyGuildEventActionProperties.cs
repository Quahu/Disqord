using System;

namespace Disqord.Rest
{
    public sealed class ModifyGuildEventActionProperties
    {
        public Optional<Snowflake> ChannelId { internal get; set; }

        public Optional<string> Name { internal get; set; }

        public Optional<PrivacyLevel> PrivacyLevel { internal get; set; }

        public Optional<DateTimeOffset> ScheduledStartTime { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<GuildEventTargetType> EntityType { internal get; set; }
    }
}

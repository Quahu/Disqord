using System;

namespace Disqord.Rest
{
    public class ModifyGuildEventActionProperties
    {
        public Optional<Snowflake> ChannelId { internal get; set; }

        public Optional<string> Name { internal get; set; }

        public Optional<StagePrivacyLevel> PrivacyLevel { internal get; set; }

        public Optional<DateTimeOffset> ScheduledStartTime { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<GuildEventTarget> EntityType { internal get; set; }
    }
}

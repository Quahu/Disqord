﻿namespace Disqord
{
    public sealed class ModifyGuildProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> VoiceRegionId { internal get; set; }

        public Optional<VerificationLevel> VerificationLevel { internal get; set; }

        public Optional<DefaultNotificationLevel> DefaultNotificationLevel { internal get; set; }

        public Optional<ContentFilterLevel> ContentFilterLevel { internal get; set; }

        public Optional<Snowflake> AfkChannelId { internal get; set; }

        public Optional<int> AfkTimeout { internal get; set; }

        public Optional<LocalAttachment> Icon { internal get; set; }

        public Optional<Snowflake> OwnerId { internal get; set; }

        public Optional<LocalAttachment> Splash { internal get; set; }

        public Optional<Snowflake> SystemChannelId { internal get; set; }

        internal ModifyGuildProperties()
        { }
    }
}

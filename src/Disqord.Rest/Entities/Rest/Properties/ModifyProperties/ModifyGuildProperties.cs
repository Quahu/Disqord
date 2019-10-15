namespace Disqord
{
    public sealed class ModifyGuildProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> VoiceRegionId { internal get; set; }

        public Optional<VerificationLevel> VerificationLevel { internal get; set; }

        public Optional<DefaultNotificationLevel> DefaultMessageNotificationLevel { internal get; set; }

        public Optional<ExplicitFilterLevel> ExplicitContentFilterLevel { internal get; set; }

        public Optional<ulong> AFKChannelId { internal get; set; }

        public Optional<int> AFKTimeout { internal get; set; }

        public Optional<LocalAttachment> Icon { internal get; set; }

        public Optional<ulong> OwnerId { internal get; set; }

        public Optional<LocalAttachment> Splash { internal get; set; }

        public Optional<ulong> SystemChannelId { internal get; set; }

        internal ModifyGuildProperties()
        { }
    }
}

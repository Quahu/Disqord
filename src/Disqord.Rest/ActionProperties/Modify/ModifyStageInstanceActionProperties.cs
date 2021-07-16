namespace Disqord
{
    public sealed class ModifyStageInstanceActionProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<PrivacyLevel> PrivacyLevel { internal get; set; }

        internal ModifyStageInstanceActionProperties()
        { }
    }
}

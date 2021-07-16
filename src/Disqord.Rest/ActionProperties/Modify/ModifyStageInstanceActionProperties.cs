namespace Disqord
{
    public sealed class ModifyStageInstanceActionProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<StagePrivacyLevel> PrivacyLevel { internal get; set; }

        internal ModifyStageInstanceActionProperties()
        { }
    }
}

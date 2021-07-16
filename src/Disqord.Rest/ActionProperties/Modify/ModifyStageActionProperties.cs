namespace Disqord
{
    public sealed class ModifyStageActionProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<StagePrivacyLevel> PrivacyLevel { internal get; set; }

        internal ModifyStageActionProperties()
        { }
    }
}

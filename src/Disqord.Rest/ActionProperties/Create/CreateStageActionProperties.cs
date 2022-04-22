using Qommon;

namespace Disqord
{
    public sealed class CreateStageActionProperties
    {
        public Optional<PrivacyLevel> PrivacyLevel { internal get; set; }

        public Optional<bool> NotifyEveryone { internal get; set; }
    }
}

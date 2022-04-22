using Qommon;

namespace Disqord
{
    public sealed class CreateVoiceChannelActionProperties : CreateNestedChannelActionProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> MemberLimit { internal get; set; }

        public Optional<string> Region { internal get; set; }

        internal CreateVoiceChannelActionProperties()
        { }
    }
}

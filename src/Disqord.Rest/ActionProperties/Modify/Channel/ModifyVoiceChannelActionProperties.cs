using Qommon;

namespace Disqord
{
    public class ModifyVoiceChannelActionProperties : ModifyNestableChannelActionProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> MemberLimit { internal get; set; }

        public Optional<string> Region { internal get; set; }

        public Optional<VideoQualityMode> VideoQualityMode { internal get; set; }

        internal ModifyVoiceChannelActionProperties()
        { }
    }
}

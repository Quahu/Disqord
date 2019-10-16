namespace Disqord
{
    public class ModifyVoiceChannelProperties : ModifyNestedChannelProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> UserLimit { internal get; set; }

        internal ModifyVoiceChannelProperties()
        { }
    }
}

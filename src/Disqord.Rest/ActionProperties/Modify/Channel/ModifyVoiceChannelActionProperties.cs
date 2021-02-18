namespace Disqord
{
    public class ModifyVoiceChannelActionProperties : ModifyNestableChannelActionProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> UserLimit { internal get; set; }

        internal ModifyVoiceChannelActionProperties()
        { }
    }
}

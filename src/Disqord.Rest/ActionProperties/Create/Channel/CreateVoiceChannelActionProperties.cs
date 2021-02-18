namespace Disqord
{
    public sealed class CreateVoiceChannelActionProperties : CreateNestedChannelActionProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> UserLimit { internal get; set; }

        internal CreateVoiceChannelActionProperties()
        { }
    }
}

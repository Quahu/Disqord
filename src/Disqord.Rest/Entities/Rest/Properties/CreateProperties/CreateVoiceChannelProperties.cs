namespace Disqord
{
    public sealed class CreateVoiceChannelProperties : CreateNestedChannelProperties
    {
        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> UserLimit { internal get; set; }

        internal CreateVoiceChannelProperties()
        { }
    }
}

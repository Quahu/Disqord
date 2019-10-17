namespace Disqord
{
    public interface IVoiceChannel : INestedChannel
    {
        int UserLimit { get; }

        int Bitrate { get; }
    }
}

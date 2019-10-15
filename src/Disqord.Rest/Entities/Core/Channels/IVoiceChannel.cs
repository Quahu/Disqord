namespace Disqord
{
    public interface IVoiceChannel : IGuildChannel
    {
        int UserLimit { get; }

        int Bitrate { get; }
    }
}

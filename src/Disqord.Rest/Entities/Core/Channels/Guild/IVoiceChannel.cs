namespace Disqord
{
    public partial interface IVoiceChannel : INestedChannel
    {
        int UserLimit { get; }

        int Bitrate { get; }
    }
}

namespace Disqord
{
    public partial interface IVoiceChannel : INestedChannel
    {
        int MemberLimit { get; }

        int Bitrate { get; }
    }
}

namespace Disqord.Events
{
    public sealed class MemberJoinedEventArgs : DiscordEventArgs
    {
        public CachedMember Member { get; }

        internal MemberJoinedEventArgs(CachedMember member) : base(member.Client)
        {
            Member = member;
        }
    }
}

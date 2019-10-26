namespace Disqord.Events
{
    public sealed class MemberUpdatedEventArgs : DiscordEventArgs
    {
        public CachedMember OldMember { get; }

        public CachedMember NewMember { get; }

        internal MemberUpdatedEventArgs(CachedMember oldMember, CachedMember newMember) : base(newMember.Client)
        {
            OldMember = oldMember;
            NewMember = newMember;
        }
    }
}

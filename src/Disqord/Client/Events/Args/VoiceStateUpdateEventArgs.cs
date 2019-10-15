namespace Disqord.Events
{
    public sealed class VoiceStateUpdatedEventArgs : DiscordEventArgs
    {
        public CachedMember OldMember { get; }

        public CachedMember NewMember { get; }

        internal VoiceStateUpdatedEventArgs(CachedMember oldMember, CachedMember newMember) : base(newMember.Client)
        {
            OldMember = oldMember;
            NewMember = newMember;
        }
    }
}
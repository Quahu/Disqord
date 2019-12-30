namespace Disqord.Events
{
    public sealed class VoiceStateUpdatedEventArgs : DiscordEventArgs
    {
        public CachedMember Member { get; }

        public VoiceState OldVoiceState { get; }

        public VoiceState NewVoiceState { get; }

        internal VoiceStateUpdatedEventArgs(CachedMember member, VoiceState oldVoiceState) : base(member.Client)
        {
            Member = member;
            OldVoiceState = oldVoiceState;
            NewVoiceState = member.VoiceState?.Clone();
        }
    }
}
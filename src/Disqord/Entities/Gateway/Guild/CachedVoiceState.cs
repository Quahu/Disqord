namespace Disqord
{
    public sealed class CachedVoiceState : CachedDiscordEntity
    {
        public CachedMember Member { get; }

        internal CachedVoiceState(CachedMember member) : base(member.Client)
        {

        }
    }
}

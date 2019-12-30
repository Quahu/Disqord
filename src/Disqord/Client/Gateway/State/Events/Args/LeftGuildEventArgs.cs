namespace Disqord.Events
{
    public sealed class LeftGuildEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        internal LeftGuildEventArgs(CachedGuild guild) : base(guild.Client)
        {
            Guild = guild;
        }
    }
}

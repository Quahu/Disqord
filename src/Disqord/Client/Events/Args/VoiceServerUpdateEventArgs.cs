namespace Disqord.Events
{
    public sealed class VoiceServerUpdatedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public string Token { get; }

        public string Endpoint { get; }

        internal VoiceServerUpdatedEventArgs(CachedGuild guild, string token, string endpoint) : base(guild.Client)
        {
            Guild = guild;
            Token = token;
            Endpoint = endpoint;
        }
    }
}
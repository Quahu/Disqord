namespace Disqord.Gateway
{
    public readonly partial struct GatewayIntents
    {
        public const ulong ALL_INTENTS_VALUE = (ulong) (
            GatewayIntent.Guilds
            | GatewayIntent.Members
            | GatewayIntent.Bans
            | GatewayIntent.Emojis
            | GatewayIntent.Integrations
            | GatewayIntent.Webhooks
            | GatewayIntent.Invites
            | GatewayIntent.VoiceStates
            | GatewayIntent.Presences
            | GatewayIntent.GuildMessages
            | GatewayIntent.GuildReactions
            | GatewayIntent.GuildTyping
            | GatewayIntent.DirectMessages
            | GatewayIntent.DirectReactions
            | GatewayIntent.DirectTyping);

        public const ulong UNPRIVILEGED_INTENTS_VALUE = (ulong) (
            GatewayIntent.Guilds
            | GatewayIntent.Bans
            | GatewayIntent.Emojis
            | GatewayIntent.Integrations
            | GatewayIntent.Webhooks
            | GatewayIntent.Invites
            | GatewayIntent.VoiceStates
            | GatewayIntent.GuildMessages
            | GatewayIntent.GuildReactions
            | GatewayIntent.GuildTyping
            | GatewayIntent.DirectMessages
            | GatewayIntent.DirectReactions
            | GatewayIntent.DirectTyping);
    }
}

namespace Disqord.Gateway
{
    public readonly partial struct GatewayIntents
    {
        private const ulong AllIntentsValue = (ulong) (
            GatewayIntent.Guilds
            | GatewayIntent.Members
            | GatewayIntent.Bans
            | GatewayIntent.EmojisAndStickers
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
            | GatewayIntent.DirectTyping
            | GatewayIntent.GuildEvents);

        private const ulong UnprivilegedIntentsValue = (ulong) (
            GatewayIntent.Guilds
            | GatewayIntent.Bans
            | GatewayIntent.EmojisAndStickers
            | GatewayIntent.Integrations
            | GatewayIntent.Webhooks
            | GatewayIntent.Invites
            | GatewayIntent.VoiceStates
            | GatewayIntent.GuildMessages
            | GatewayIntent.GuildReactions
            | GatewayIntent.GuildTyping
            | GatewayIntent.DirectMessages
            | GatewayIntent.DirectReactions
            | GatewayIntent.DirectTyping
            | GatewayIntent.GuildEvents);

        private const ulong RecommendedValue = (ulong) (
            GatewayIntent.Guilds
            | GatewayIntent.Members
            | GatewayIntent.Bans
            | GatewayIntent.EmojisAndStickers
            | GatewayIntent.Integrations
            | GatewayIntent.Webhooks
            | GatewayIntent.Invites
            | GatewayIntent.VoiceStates
            | GatewayIntent.GuildMessages
            | GatewayIntent.GuildReactions
            | GatewayIntent.GuildEvents);
    }
}

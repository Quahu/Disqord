namespace Disqord.Tests.Integration;

public sealed class DiscordIntegrationTestAttribute() : ExplicitAttribute("explicit: Requires DISQORD_INTEGRATION_BOT_TOKEN, DISQORD_INTEGRATION_BOT_GUILD_ID");

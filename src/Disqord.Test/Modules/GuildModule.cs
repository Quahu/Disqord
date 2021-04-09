using Disqord.Bot;
using Qmmands;

namespace Disqord.Test.Modules
{
    [Group("guild")]
    [RequireGuild]
    public class GuildModule : DiscordModuleBase<DiscordGuildCommandContext>
    {
        [Command("test")]
        public DiscordCommandResult Test()
            => Reply("guild test");
    }
}

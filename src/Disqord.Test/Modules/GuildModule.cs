using Disqord.Bot;
using Qmmands;

namespace Disqord.Test.Modules
{
    [Group("guild")]
    public class GuildModule : DiscordGuildModuleBase
    {
        [Command("test")]
        public DiscordCommandResult Test()
            => Reply("guild only test");
    }
}

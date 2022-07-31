using Disqord.Bot.Commands.Text;
using Qmmands;
using Qmmands.Text;

namespace Disqord.Test.Modules
{
    [TextGroup("guild")]
    public class GuildModule : DiscordTextGuildModuleBase
    {
        [TextCommand("test")]
        public IResult Test()
        {
            return Reply("guild only test");
        }
    }
}

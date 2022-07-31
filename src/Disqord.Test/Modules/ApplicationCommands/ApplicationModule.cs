using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Qmmands;

namespace Disqord.Test.Modules.ApplicationCommands
{
    public class ApplicationModule : DiscordApplicationModuleBase
    {
        // API enabled globally, but fails in guilds because of the check
        [SlashCommand("private-only")]
        [RequirePrivate]
        public IResult PrivateOnly()
        {
            return Response("Success!");
        }

        // API enabled in guilds only
        [SlashCommand("guild-only")]
        [RequireGuild]
        public IResult GuildOnly()
        {
            return Response("Success!");
        }
    }
}

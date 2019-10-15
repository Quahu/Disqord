using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class GuildOwnerOnlyAttribute : GuildOnlyAttribute
    {
        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            return context.User.Id == context.Guild.OwnerId
                ? CheckResult.Successful
                : CheckResult.Unsuccessful("This command can only be used by the guild's owner.");
        }
    }
}

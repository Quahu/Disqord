using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireNotBotAttribute : DiscordParameterCheckAttribute
    {
        public override bool CheckType(Type type)
            => typeof(IUser).IsAssignableFrom(type);

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordCommandContext context)
        {
            if (argument is IUser user && !user.IsBot)
                return Success();

            return Failure("The provided argument must not be a bot user.");
        }
    }
}

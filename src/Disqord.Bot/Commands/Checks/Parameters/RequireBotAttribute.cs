using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the <see cref="IUser"/> parameter must be a bot.
    /// </summary>
    public class RequireBotAttribute : DiscordParameterCheckAttribute
    {
        public override bool CheckType(Type type)
            => typeof(IUser).IsAssignableFrom(type);

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordCommandContext context)
        {
            var user = argument as IUser;
            if (user.IsBot)
                return Success();

            return Failure("The provided argument must be a bot user.");
        }
    }
}

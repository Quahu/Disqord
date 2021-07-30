using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the <see cref="IUser"/> parameter must not be the author.
    /// </summary>
    public class RequireNotAuthorAttribute : DiscordParameterCheckAttribute
    {
        public override bool CheckType(Type type)
            => typeof(IUser).IsAssignableFrom(type);

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordCommandContext context)
        {
            if (argument is IUser user && user.Id != context.Author.Id)
                return Success();

            return Failure("The provided argument must be another user.");
        }
    }
}

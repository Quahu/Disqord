using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireAuthorChannelPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public Permission Permissions { get; }

        public RequireAuthorChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.Author.GetChannelPermissions(context.Guild, context.Channel);
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"You lack the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

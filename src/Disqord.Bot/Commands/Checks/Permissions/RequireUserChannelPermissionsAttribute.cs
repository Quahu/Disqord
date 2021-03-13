using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireUserChannelPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public Permission Permissions { get; }

        public RequireUserChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var roles = context.Author.GetRoles();
            var permissions = Discord.Permissions.CalculatePermissions(context.Guild, context.Channel, context.Author, roles.Values);
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"You lack the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireAuthorGuildPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public GuildPermissions Permissions { get; }

        public RequireAuthorGuildPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.Author.GetPermissions();
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"You lack the necessary guild permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

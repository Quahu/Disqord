using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed by authors with the given guild permissions.
    /// </summary>
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

            return Failure($"You lack the necessary guild permissions ({Permissions & ~permissions}) to execute this.");
        }
    }
}

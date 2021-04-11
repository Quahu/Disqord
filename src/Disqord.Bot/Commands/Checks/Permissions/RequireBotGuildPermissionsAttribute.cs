using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireBotGuildPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public Permission Permissions { get; }

        public RequireBotGuildPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.CurrentMember.GetGuildPermissions(context.Guild);
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"The bot lacks the necessary guild permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

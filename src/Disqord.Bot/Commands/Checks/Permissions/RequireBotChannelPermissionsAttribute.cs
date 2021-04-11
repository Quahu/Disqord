using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireBotChannelPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public Permission Permissions { get; }

        public RequireBotChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.CurrentMember.GetChannelPermissions(context.Guild, context.Channel);
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"The bot lacks the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

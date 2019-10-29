using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireMemberChannelPermissionsAttribute : GuildOnlyAttribute
    {
        public ChannelPermissions Permissions { get; }

        public RequireMemberChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public RequireMemberChannelPermissionsAttribute(params Permission[] permissions)
        {
            if (permissions == null)
                throw new ArgumentNullException(nameof(permissions));

            Permissions = permissions.Aggregate(Permission.None, (total, permission) => total | permission);
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            var permissions = context.Member.GetPermissionsFor(context.Channel as IGuildChannel);
            return permissions.Has(Permissions)
                ? CheckResult.Successful
                : CheckResult.Unsuccessful($"You lack the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

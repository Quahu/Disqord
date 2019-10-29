using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireBotChannelPermissionsAttribute : GuildOnlyAttribute
    {
        public ChannelPermissions Permissions { get; }

        public RequireBotChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public RequireBotChannelPermissionsAttribute(params Permission[] permissions)
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
            var permissions = context.Guild.CurrentMember.GetPermissionsFor(context.Channel as IGuildChannel);
            return permissions.Has(Permissions)
                ? CheckResult.Successful
                : CheckResult.Unsuccessful($"The bot lacks the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

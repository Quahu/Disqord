using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed if the bot has given channel permissions.
    /// </summary>
    public class RequireBotChannelPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public ChannelPermissions Permissions { get; }

        public RequireBotChannelPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.CurrentMember.GetPermissions(context.Channel);
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"The bot lacks the necessary channel permissions ({Permissions - permissions}) to execute this.");
        }
    }
}

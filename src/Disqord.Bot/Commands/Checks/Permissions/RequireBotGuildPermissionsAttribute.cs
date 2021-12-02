using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed if the bot has given guild permissions.
    /// </summary>
    public class RequireBotGuildPermissionsAttribute : DiscordGuildCheckAttribute
    {
        public GuildPermissions Permissions { get; }

        public RequireBotGuildPermissionsAttribute(Permission permissions)
        {
            Permissions = permissions;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            var permissions = context.CurrentMember.GetPermissions();
            if (permissions.Has(Permissions))
                return Success();

            return Failure($"The bot lacks the necessary guild permissions ({Permissions & ~permissions}) to execute this.");
        }
    }
}

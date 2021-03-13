using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireRoleAttribute : DiscordGuildCheckAttribute
    {
        public Snowflake Id { get; }

        public RequireRoleAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (context.Author.RoleIds.Any(x => x == Id))
                return Success();

            return Failure($"This can only be executed by members with the role ID {Id}.");
        }
    }
}

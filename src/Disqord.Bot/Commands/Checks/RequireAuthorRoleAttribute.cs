using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed by authors with the given role.
    /// </summary>
    public class RequireAuthorRoleAttribute : DiscordGuildCheckAttribute
    {
        public Snowflake Id { get; }

        public RequireAuthorRoleAttribute(ulong id)
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

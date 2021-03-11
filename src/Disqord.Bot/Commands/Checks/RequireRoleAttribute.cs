using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireRoleAttribute : RequireGuildAttribute
    {
        public new Snowflake Id { get; }

        public RequireRoleAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var task = base.CheckAsync(_);
            Debug.Assert(task.IsCompleted);
            var result = task.Result;
            if (!result.IsSuccessful)
                return result;

            var context = _ as DiscordGuildCommandContext;
            if (context.Author.RoleIds.Any(x => x == Id))
                return Success();

            return Failure($"This can only be executed by members with the role ID {Id}.");
        }
    }
}

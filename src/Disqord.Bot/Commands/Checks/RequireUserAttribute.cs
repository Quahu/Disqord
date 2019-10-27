using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireUserAttribute : CheckAttribute
    {
        public Snowflake Id { get; }

        public RequireUserAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            return Id == context.User.Id
                ? CheckResult.Successful
                : CheckResult.Unsuccessful("You are not authorized to execute this.");
        }
    }
}

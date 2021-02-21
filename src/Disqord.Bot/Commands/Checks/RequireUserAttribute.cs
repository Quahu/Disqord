using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireUserAttribute : CheckAttribute
    {
        public Snowflake Id { get; }

        public RequireUserAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            if (context.Author.Id == Id)
                return Success();

            return Failure($"This can only be executed by the user with the ID {Id}.");
        }
    }
}

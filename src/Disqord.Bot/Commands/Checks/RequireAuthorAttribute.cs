using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireAuthorAttribute : DiscordCheckAttribute
    {
        public Snowflake Id { get; }

        public RequireAuthorAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordCommandContext context)
        {
            if (context.Author.Id == Id)
                return Success();

            return Failure($"This can only be executed by the user with the ID {Id}.");
        }
    }
}

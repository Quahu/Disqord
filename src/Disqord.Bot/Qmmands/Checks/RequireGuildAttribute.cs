using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireGuildAttribute : CheckAttribute
    {
        public Snowflake? Id { get; }

        public RequireGuildAttribute()
        { }

        public RequireGuildAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            if (context.GuildId != null)
            {
                if (Id != null)
                {
                    if (context.Message.GuildId == Id)
                        return Success();

                    return Failure($"This can only be executed in the guild with the ID {Id}.");
                }
                else
                {
                    return Success();
                }
            }

            return Failure("This can only be executed within a guild.");
        }
    }
}

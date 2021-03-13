using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireGuildAttribute : DiscordGuildCheckAttribute
    {
        public Snowflake? Id { get; }

        public RequireGuildAttribute()
        { }

        public RequireGuildAttribute(ulong id)
        {
            Id = id;
        }

        public override sealed ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (Id != null)
            {
                if (context.GuildId == Id)
                    return Success();

                return Failure($"This can only be executed in the guild with the ID {Id}.");
            }

            return Success();
        }
    }
}

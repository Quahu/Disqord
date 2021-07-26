using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed within a guild.
    ///     If the <see cref="Id"/> is specified, then execution is limited to the given guild.
    /// </summary>
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
            if (Id == null || Id == context.GuildId)
                return Success();

            return Failure($"This can only be executed in the guild with the ID {Id}.");
        }
    }
}

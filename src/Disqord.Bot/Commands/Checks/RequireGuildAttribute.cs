using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireGuildAttribute : GuildOnlyAttribute
    {
        public Snowflake Id { get; }

        public RequireGuildAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            return Id == context.Guild.Id
                ? CheckResult.Successful
                : CheckResult.Unsuccessful("This cannot be executed in this guild.");
        }
    }
}

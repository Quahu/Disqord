using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireMemberAttribute : GuildOnlyAttribute
    {
        public Snowflake Id { get; }

        public RequireMemberAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            return Id == context.Member.Id
                ? CheckResult.Successful
                : CheckResult.Unsuccessful("You are not authorized to execute this.");
        }
    }
}

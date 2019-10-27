using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireBotRoleAttribute : GuildOnlyAttribute
    {
        public Snowflake Id { get; }

        public RequireBotRoleAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            return context.Guild.CurrentMember.Roles.ContainsKey(Id)
                ? CheckResult.Successful
                : CheckResult.Unsuccessful($"The bot does not have the required role {Id}.");

        }
    }
}

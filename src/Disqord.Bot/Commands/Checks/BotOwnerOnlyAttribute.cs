using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class BotOwnerOnlyAttribute : CheckAttribute
    {
        public BotOwnerOnlyAttribute()
        { }

        public override async ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            switch (context.Bot.TokenType)
            {
                case TokenType.Bot:
                {
                    return (await context.Bot.CurrentApplication.GetAsync().ConfigureAwait(false)).Owner.Id == context.User.Id
                        ? CheckResult.Successful
                        : CheckResult.Unsuccessful("This can only be executed by the bot's owner.");
                }

                case TokenType.Bearer:
                case TokenType.User:
                {
                    return context.Bot.CurrentUser.Id == context.User.Id
                        ? CheckResult.Successful
                        : CheckResult.Unsuccessful("This can only be executed by the currently logged in user.");
                }

                default:
                    throw new InvalidOperationException("Invalid token type.");
            }
        }
    }
}

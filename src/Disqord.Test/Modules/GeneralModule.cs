using System.Threading.Tasks;
using Disqord.Bot;
using Qmmands;

namespace Disqord.Test.Modules
{
    public sealed class GeneralModule : DiscordModuleBase
    {
        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("pong");

        [Command("file")]
        public async Task FileAsync()
        {
            using (var attachment = new LocalAttachment(@"C:\Users\quahu\Desktop\bbb.png"))
            {
                await ReplyAsync(attachment);
            }
        }

        [Group("get")]
        public sealed class GetModule : DiscordModuleBase
        {
            [Command("ban")]
            public async Task GetBanAsync(Snowflake id)
            {
                var ban = await Context.Bot.GetBanAsync(Context.Guild.Id, id);
                if (ban == null)
                    await ReplyAsync($"`{id}` isn't banned.");
                else
                    await ReplyAsync($"`{id}` ({ban.User}) is banned.");
            }
        }
    }
}

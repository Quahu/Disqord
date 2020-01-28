using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
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

        [Command("paged")]
        public async Task PagedMenuAsync()
        {
            var pages = new Page[]
            {
                /* string */ "First page!",
                /* embed  */ new LocalEmbedBuilder().WithDescription("Second page!").Build(),
                /* tuple  */ ("Third page!", new LocalEmbedBuilder().WithAuthor(Context.User).Build())
            };
            var pageProvider = new DefaultPageProvider(pages);
            var menu = new PagedMenu(Context.User.Id, pageProvider);
            await Context.Channel.StartMenuAsync(menu);
        }

        [Command("pagedmembers")]
        public async Task PagedMembersMenuAsync()
        {
            var members = Context.Guild.Members.Values as CachedMember[];
            var pageProvider = new ArrayPageProvider<CachedMember>(members);
            var menu = new PagedMenu(Context.User.Id, pageProvider);
            await Context.Channel.StartMenuAsync(menu);
        }
    }
}

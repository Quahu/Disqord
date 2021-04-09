using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Gateway;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Test
{
    public class TestModule : DiscordModuleBase
    {
        [Command("responses")]
        public async Task<DiscordCommandResult> Responses()
        {
            var message = await Response("1");
            await Response("2");
            return Response("3");
        }

        [Command("waitmessage")]
        public async Task<DiscordCommandResult> WaitMessage()
        {
            var random = new Random();
            var number = random.Next(0, 10).ToString();
            await Response($"Send dis: {number}");
            var e = await Context.WaitForMessageAsync(x => x.Message.Content == number);
            return Response(e != null
                ? "Correct!!!"
                : "You didn't say anything...");
        }

        [Command("pages")]
        public DiscordCommandResult Pages()
        {
            Page page1 = "First page!";
            Page page2 = new LocalEmbedBuilder().WithDescription("Second page!");
            Page page3 = ("Third page!", new LocalEmbedBuilder().WithAuthor(Context.Author.Tag));
            return Pages(page1, page2, page3);
        }

        [Command("vote")]
        public DiscordCommandResult Vote()
        {
            return Menu(new VoteMenu());
        }

        public sealed class VoteMenu : MenuBase
        {
            protected override async Task<Snowflake> InitializeAsync()
            {
                var message = await Client.SendMessageAsync(ChannelId, new LocalMessageBuilder
                {
                    Content = "pls vote"
                }.Build());
                return message.Id;
            }

            [Button("👍")]
            public Task Upvote(ButtonEventArgs e)
            {
                var content = e.WasAdded
                    ? "thanks for the support"
                    : ":frowning:";
                return e.Message.ModifyAsync(x => x.Content = content);
            }

            [Button("👎")]
            public Task Downvote(ButtonEventArgs e)
            {
                var content = !e.WasAdded
                    ? "thanks for changing ur mind"
                    : ":frowning2:";
                return e.Message.ModifyAsync(x => x.Content = content);
            }
        }

        [Command("paged2")]
        public async Task Paged2()
        {
            var interactivity = Context.Bot.GetExtension<InteractivityExtension>();
            var strings = Enumerable.Range(0, 100).Select(x => new string('a', x)).ToArray();
            var pageProvider = new ArrayPageProvider<string>(strings);
            var menu = new PagedMenu(Context.Author.Id, pageProvider);
            await interactivity.StartMenuAsync(Context.ChannelId, menu);
        }

        [Command("shard")]
        [Description("Displays the shard for this context.")]
        public DiscordCommandResult Shard()
        {
            var shard = Context.Bot.GetShard(Context.GuildId);
            return Response($"This is {shard.Id} speaking.");
        }

        [Command("ping")]
        [Cooldown(1, 5, CooldownMeasure.Seconds, CooldownBucketType.Channel)]
        public DiscordCommandResult Ping()
            => Response("pong");

        [Command("react")]
        public DiscordCommandResult React()
            => Reaction(new LocalEmoji("🚿"));

        [Command("id")]
        public DiscordCommandResult Id(Snowflake id)
            => Response($"{id}: {id.CreatedAt}");
    }
}

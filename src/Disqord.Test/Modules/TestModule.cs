using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Test
{
    public class TestModule : DiscordModuleBase
    {
        [Command("yield")]
        public async Task<DiscordCommandResult> Yield()
        {
            Context.Yield();
            await Task.Delay(10_000);
            return Reply("hihi");
        }

        [Command("responses")]
        public async Task<DiscordCommandResult> Responses()
        {
            await Response("1");
            await Response("2");
            return Response("3");
        }

        [Command("waitmessage")]
        public async Task WaitMessage()
        {
            var random = new Random();
            var number = random.Next(0, 10).ToString();
            await Response($"Send dis: {number}");
            var interactivity = Context.Bot.GetExtension<InteractivityExtension>();
            var e = await interactivity.WaitForMessageAsync(Context.ChannelId, x => x.Message.Content == number);
            await Response(e != null
                ? "Correct!!!"
                : "You didn't say anything...");
        }

        [Command("paged")]
        public async Task PagedAsync()
        {
            var interactivity = Context.Bot.GetExtension<InteractivityExtension>();
            var pages = new Page[]
            {
                /* string */ "First page!",
                /* embed  */ new LocalEmbedBuilder().WithDescription("Second page!"),
                /* tuple  */ ("Third page!", new LocalEmbedBuilder().WithAuthor(Context.Author.Tag))
            };
            var pageProvider = new DefaultPageProvider(pages);
            var menu = new PagedMenu(Context.Author.Id, pageProvider);
            await interactivity.StartMenuAsync(Context.ChannelId, menu);
        }

        [Command("vote")]
        public async Task VoteAsync()
        {
            var interactivity = Context.Bot.GetExtension<InteractivityExtension>();
            var menu = new VoteMenu();
            await interactivity.StartMenuAsync(Context.ChannelId, menu);
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
        public async Task PagedMembersMenuAsync()
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
            var shard = Context.Bot.GatewayClient.GetShard(Context.GuildId);
            return Response($"This is {shard.Id} speaking.");
        }

        [Command("ping")]
        [Cooldown(1, 5, CooldownMeasure.Seconds, CooldownBucketType.Channel)]
        public DiscordCommandResult Ping()
            => Response("pong");

        [Command("react")]
        public DiscordCommandResult React()
            => Reaction(new LocalEmoji("🚿"));

        [Command("color")]
        public DiscordCommandResult Color([Remainder] Color color)
            => Response(new LocalEmbedBuilder()
                .WithDescription(color.ToString())
                .WithColor(color));

        [Command("id")]
        public DiscordCommandResult Id(Snowflake id)
            => Response($"{id}: {id.CreatedAt}");

        [Command("reply")]
        public DiscordCommandResult Reply()
            => Reply("hi");

        [Command("replynoping")]
        public DiscordCommandResult ReplyNoPing()
            => Reply("hi", LocalMentionsBuilder.ExceptRepliedUser);

        [Command("long")]
        public async Task<DiscordCommandResult> Long()
        {
            await Task.Delay(10_000);
            return Response("Hello after 10 seconds");
        }
        
        [Command("parallel")]
        [RunMode(RunMode.Parallel)]
        public void Parallel()
        { }

        [Command("exception")]
        public void Exception()
            => throw new Exception("cool exception");
    }
}

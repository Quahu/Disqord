using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Gateway;
using Disqord.Models;
using Disqord.Rest;
using Disqord.Rest.Api;
using Qmmands;

namespace Disqord.Test
{
    public class TestModule : DiscordModuleBase
    {
        [Command("slash")]
        [RequireBotOwner]
        public async ValueTask Slash()
        {
            var api = (Bot as IRestClient).ApiClient;
            var content = new CreateApplicationCommandJsonRestRequestContent
            {
                // slash command creation
                Name = "echo",
                Description = "Echoes user input.",
                Options = new[]
                {
                    new ApplicationCommandOptionJsonModel
                    {
                        Name = "text",
                        Description = "The text to echo.",
                        Required = true,
                        Type = SlashCommandOptionType.String
                    }
                }

                // context menu command creation
                // Name = "Rate Message",
                // Type = ApplicationCommandType.Message
            };
            await api.CreateGuildApplicationCommandAsync(Bot.CurrentUser.Id, Context.GuildId.Value, content);
        }

        [Command("menudemo")]
        public DiscordCommandResult MenuDemo()
            => View(new FirstView());

        public class FirstView : PagedViewBase
        {
            public FirstView()
                : base(new ListPageProvider(new[]
                {
                    new Page().WithContent("This is the first view! Click the button below to continue onto the next page."),
                    new Page().WithContent("🎉 You successfully navigated to the second page. Click again!"),
                    new Page().WithEmbeds(new LocalEmbed().WithDescription("You made it to the THIRD page. Click again to see the next view."))
                }))
            { }

            [Button(Label = "Continue")]
            public ValueTask Continue(ButtonEventArgs e)
            {
                if (CurrentPageIndex + 1 == PageProvider.PageCount) // If the user clicked continue on the last page, switch the view.
                    Menu.View = new SecondView();

                CurrentPageIndex++;
                return default;
            }
        }

        public class SecondView : ViewBase
        {
            private int _clicks;
            private readonly ButtonViewComponent _clicker;

            public SecondView()
                : base(new LocalMessage()
                    .WithEmbeds(new LocalEmbed()
                        .WithDescription("This is the second view!")))
            {
                _clicker = new ButtonViewComponent(e =>
                {
                    e.Button.Label = $"{++_clicks} {(_clicks == 1 ? "click" : "clicks")}";
                    return default;
                })
                {
                    Label = "Click Me!",
                    Style = LocalButtonComponentStyle.Success,
                    Row = 1,
                    Position = 0
                };
            }

            [Button(Label = "Randomize Color", Emoji = "🎨")]
            public ValueTask RandomizeColor(ButtonEventArgs e)
            {
                TemplateMessage.Embeds[0].Color = Color.Random;
                ReportChanges(); // The template message's properties aren't tracked, so we have to report them.
                return default;
            }

            [Button(Label = "Toggle Clicker", Row = 1, Style = LocalButtonComponentStyle.Secondary)]
            public ValueTask ToggleClicker(ButtonEventArgs e)
            {
                var isClickerEnabled = EnumerateComponents().Any(x => x == _clicker);
                if (!isClickerEnabled)
                {
                    e.Button.Emoji = LocalEmoji.Unicode("👈"); // Points at the clicker to the left.
                    AddComponent(_clicker);
                }
                else
                {
                    e.Button.Emoji = null; // Removes the pointer emoji.
                    RemoveComponent(_clicker);
                }

                return default;
            }
        }

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
            var page1 = new Page()
                .WithContent("First page!");
            var page2 = new Page()
                .WithEmbeds(new LocalEmbed().WithDescription("Second page!"));
            var page3 = new Page().WithContent("Third page!")
                .WithEmbeds(new LocalEmbed().WithAuthor(Context.Author.Tag));
            return Pages(page1, page2, page3);
        }

        [Command("pages2")]
        public DiscordCommandResult Paged2()
        {
            var strings = Enumerable.Range(0, 100).Select(x => new string('a', x)).ToArray();
            var pageProvider = new ArrayPageProvider<string>(strings);
            return Pages(pageProvider);
        }

        [Command("shard")]
        [Description("Displays the shard for this context.")]
        public DiscordCommandResult Shard()
        {
            var shardId = Context.Bot.GetShardId(Context.GuildId);
            return Response($"This is {shardId} speaking.");
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

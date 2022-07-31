using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Text;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Gateway;
using Qmmands;
using Qmmands.Text;
using Qommon;

namespace Disqord.Test.Modules
{
    public class TestModule : DiscordTextModuleBase
    {
        [TextCommand("menudemo")]
        public IResult MenuDemo()
        {
            return View(new FirstView());
        }

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
            private Optional<Color> _color;

            public SecondView()
                : base(null)
            {
                MessageTemplate = message =>
                {
                    message.WithEmbeds(new LocalEmbed
                    {
                        Description = "This is the second view!",
                        Color = _color
                    });
                };

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
                _color = Color.Random;
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

        [TextCommand("responses")]
        public async Task<IResult> Responses()
        {
            var message = await Response("1");
            await Response("2");
            return Response("3");
        }

        [TextCommand("waitformessage")]
        public async Task<IResult> WaitForMessage()
        {
            var random = new Random();
            var number = random.Next(0, 10).ToString();
            await Response($"Send dis: {number}");
            var e = await Context.WaitForMessageAsync(x => x.Message.Content == number);
            return Response(e != null
                ? "Correct!!!"
                : "You didn't say anything...");
        }

        [TextCommand("pages")]
        public IResult Pages()
        {
            var page1 = new Page()
                .WithContent("First page!");

            var page2 = new Page()
                .WithEmbeds(new LocalEmbed().WithDescription("Second page!"));

            var page3 = new Page().WithContent("Third page!")
                .WithEmbeds(new LocalEmbed().WithAuthor(Context.Author.Tag));

            return Pages(page1, page2, page3);
        }

        [TextCommand("arraypages")]
        public IResult ArrayPages()
        {
            var strings = Enumerable.Range(0, 100).Select(x => new string('a', x)).ToArray();
            var pageProvider = new ArrayPageProvider<string>(strings);
            return Pages(pageProvider);
        }

        [TextCommand("shard")]
        [Description("Displays the shard for this context.")]
        public IResult Shard()
        {
            var shardId = Bot.ApiClient.GetShardId(Context.GuildId);
            return Response($"This is {shardId} speaking.");
        }

        [TextCommand("ping")]
        [RateLimit(1, 5, RateLimitMeasure.Seconds, RateLimitBucketType.Channel)]
        public IResult Ping()
        {
            return Response("Pong!");
        }

        [TextCommand("react")]
        public IResult React()
        {
            return Reaction(new LocalEmoji("👍"));
        }

        [TextCommand("id", "snowflake")]
        public IResult Id(Snowflake id)
        {
            return Response($"{id}: {id.CreatedAt}");
        }
    }
}

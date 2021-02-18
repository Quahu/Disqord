using Disqord.Bot;
using Qmmands;

namespace Disqord.Test
{
    public class TestModule : DiscordModuleBase
    {
        [Command("ping")]
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
    }
}

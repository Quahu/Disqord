using System.Threading.Tasks;
using Disqord.Bot;
using Qmmands;

namespace Disqord.Test
{
    public class TestModule : DiscordModuleBase
    {
        [Command("responses")]
        public async Task<DiscordCommandResult> Responses()
        {
            await Response("1");
            await Response("2");
            return Response("3");
        }

        [Command("shard")]
        [Description("Displays the shard for this context.")]
        public DiscordCommandResult Shard()
        {
            var shard = Context.Bot.GatewayClient.GetShard(Context.GuildId);
            return Response($"This is {shard.Id} speaking.");
        }

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

        [Command("reply")]
        public DiscordCommandResult Reply()
            => Reply("hi");

        [Command("replynoping")]
        public DiscordCommandResult ReplyNoPing()
            => Reply("hi", LocalMentionsBuilder.None);
    }
}

using Disqord.Bot.Prefixes;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordBot : DiscordBotBase
    {
        public DiscordBot(RestDiscordClient restClient, IPrefixProvider prefixProvider, DiscordBotConfiguration configuration = null)
            : base(new DiscordClient(restClient, configuration ??= new DiscordBotConfiguration()), prefixProvider, configuration)
        { }

        public DiscordBot(TokenType tokenType, string token, IPrefixProvider prefixProvider, DiscordBotConfiguration configuration = null)
            : base(new DiscordClient(tokenType, token, configuration ??= new DiscordBotConfiguration()), prefixProvider, configuration)
        { }
    }
}
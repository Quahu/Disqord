using Disqord.Gateway;

namespace Disqord.Hosting
{
    public class DiscordHostingContext
    {
        public virtual string Token { get; set; }

        public virtual GatewayIntents Intents { get; set; }
    }
}

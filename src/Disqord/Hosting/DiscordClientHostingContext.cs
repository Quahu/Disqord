using Disqord.Gateway;

namespace Disqord.Hosting
{
    public class DiscordClientHostingContext
    {
        /// <summary>
        ///     Gets or sets the bot token.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if a custom <see cref="Disqord.Token"/> is registered.
        /// </remarks>
        public virtual string Token { get; set; }

        /// <summary>
        ///     Gets or sets the gateway intents.
        /// </summary>
        // TODO: default to objectively best intents
        public virtual GatewayIntents Intents { get; set; }
    }
}

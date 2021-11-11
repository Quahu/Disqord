using System.Collections.Generic;
using System.Net;
using System.Reflection;
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
        ///     Defaults to <see cref="GatewayIntents.Recommended"/> which are the intents the library recommends.
        /// </summary>
        public virtual GatewayIntents? Intents { get; set; } = GatewayIntents.Recommended;

        /// <summary>
        ///     Gets or sets the mode of firing the <see cref="DiscordClientBase.Ready"/> event.
        ///     Defaults to <see cref="Disqord.ReadyEventDelayMode.Guilds"/>.
        /// </summary>
        public virtual ReadyEventDelayMode ReadyEventDelayMode { get; set; } = ReadyEventDelayMode.Guilds;

        /// <summary>
        ///     Gets or sets the assemblies from which to register <see cref="DiscordClientService"/> implementations.
        ///     Defaults to <see cref="Assembly.GetEntryAssembly"/>.
        ///     If <see langword="null"/> or empty, the services will have to be manually registered.
        /// </summary>
        public virtual IList<Assembly> ServiceAssemblies { get; set; } = new List<Assembly> { Assembly.GetEntryAssembly() };

        /// <summary>
        ///     Gets or sets the status> the bot will identify with.
        ///     Defaults to <see langword="null"/>, which will result in <see cref="UserStatus.Online"/>.
        /// </summary>
        public virtual UserStatus? Status { get; set; }

        /// <summary>
        ///     Gets or sets the activities the bot will identify with.
        ///     Defaults to <see langword="null"/>, which will result in no activities.
        /// </summary>
        public virtual IEnumerable<LocalActivity> Activities { get; set; }

        /// <summary>
        ///     Gets or sets the proxy to use for Discord's REST API.
        /// </summary>
        public virtual IWebProxy RestProxy { get; set; }

        /// <summary>
        ///     Gets or sets the proxy to use for Discord's Gateway API.
        /// </summary>
        public virtual IWebProxy GatewayProxy { get; set; }
    }
}

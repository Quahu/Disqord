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
        public virtual string Token { get; set; }

        /// <summary>
        ///     Gets or sets the gateway intents.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see cref="GatewayIntents.Recommended"/> which are the intents the library recommends.
        ///     These include <see cref="GatewayIntent.Members"/> which is a privileged intent.
        /// </remarks>
        public virtual GatewayIntents Intents { get; set; } = GatewayIntents.Recommended;

        /// <summary>
        ///     Gets or sets the mode of delaying and firing the <see cref="DiscordClientBase.Ready"/> event.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see cref="Disqord.ReadyEventDelayMode.Guilds"/>.
        /// </remarks>
        public virtual ReadyEventDelayMode ReadyEventDelayMode { get; set; } = ReadyEventDelayMode.Guilds;

        /// <summary>
        ///     Gets or sets the assemblies from which to register <see cref="DiscordClientService"/> implementations.
        /// </summary>
        /// <remarks>
        ///     Defaults to a list containing the result of <see cref="Assembly.GetEntryAssembly"/>.
        ///     If <see langword="null"/> or empty, the services will have to be manually registered.
        /// </remarks>
        public virtual IList<Assembly> ServiceAssemblies { get; set; } = new List<Assembly> { Assembly.GetEntryAssembly() };

        /// <summary>
        ///     Gets or sets the status the bot will identify with.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="null"/>, i.e. the bot will show up as <see cref="UserStatus.Online"/>.
        /// </remarks>
        public virtual UserStatus? Status { get; set; }

        /// <summary>
        ///     Gets or sets the activities the bot will identify with.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="null"/>, i.e. no activities.
        /// </remarks>
        public virtual IEnumerable<LocalActivity> Activities { get; set; }

        /// <summary>
        ///     Gets or sets the proxy to use for Discord's REST API.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="null"/>, i.e. no proxy.
        /// </remarks>
        public virtual IWebProxy RestProxy { get; set; }

        /// <summary>
        ///     Gets or sets the proxy to use for Discord's Gateway API.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="null"/>, i.e. no proxy.
        /// </remarks>
        public virtual IWebProxy GatewayProxy { get; set; }
    }
}

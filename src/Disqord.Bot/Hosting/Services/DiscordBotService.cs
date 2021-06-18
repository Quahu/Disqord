using System.ComponentModel;
using Disqord.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot.Hosting
{
    /// <inheritdoc/>
    public abstract partial class DiscordBotService : DiscordClientService
    {
        /// <inheritdoc cref="DiscordClientService.Client"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new DiscordBotBase Client => base.Client as DiscordBotBase;

        /// <summary>
        ///     Gets the bot client of this service.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="Client"/>
        /// </remarks>
        public DiscordBotBase Bot => Client;

        /// <summary>
        ///     Gets the priority of this bot service. Defaults to <c>0</c>.
        /// </summary>
        /// <remarks>
        ///     Higher value means higher priority.
        /// </remarks>
        public virtual int Priority => 0;

        /// <summary>
        ///     Instantiates a new <see cref="DiscordBotService"/>.
        /// </summary>
        protected DiscordBotService()
        { }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordBotService"/> with the specified logger and bot client.
        /// </summary>
        /// <param name="logger"> The logger to use. </param>
        /// <param name="bot"> The bot client to use. </param>
        protected DiscordBotService(
            ILogger logger,
            DiscordBotBase bot)
            : base(logger, bot)
        { }
    }
}

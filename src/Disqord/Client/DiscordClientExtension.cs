using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Microsoft.Extensions.Logging;

namespace Disqord
{
    public abstract class DiscordClientExtension : ILogging
    {
        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the client this extension is bound to.
        /// </summary>
        /// <remarks>
        ///     This property is set when <see cref="InitialiseAsync(DiscordClientBase)"/> is called.
        /// </remarks>
        public DiscordClientBase Client { get; private set; }

        /// <summary>
        ///     Gets whether this extension is initialised.
        /// </summary>
        public bool IsInitialised { get; private set; }

        protected DiscordClientExtension(
            ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        ///     Overriddable logic for initialisation code.
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token passed from <see cref="InitialiseAsync(DiscordClientBase, CancellationToken)"/>. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the initialisation work.
        /// </returns>
        protected virtual ValueTask InitialiseAsync(CancellationToken cancellationToken)
            => default;

        /// <summary>
        ///     Initialises this extension and binds it to the provided client.
        /// </summary>
        /// <param name="client"> The calling client to bind to. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the initialisation work.
        /// </returns>
        public async Task InitialiseAsync(DiscordClientBase client, CancellationToken cancellationToken = default)
        {
            if (IsInitialised)
                throw new InvalidOperationException($"This extension ({GetType().Name}) has already been initialised.");

            Client = client;
            await InitialiseAsync(cancellationToken).ConfigureAwait(false);
            IsInitialised = true;
        }
    }
}

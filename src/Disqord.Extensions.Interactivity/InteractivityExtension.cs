using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Extensions.Interactivity
{
    public class InteractivityExtension : DiscordClientExtension
    {
        //private readonly ISynchronizedDictionary<Snowflake, >

        public InteractivityExtension(
            IOptions<InteractivityExtensionConfiguration> options,
            ILogger<InteractivityExtension> logger)
            : base(logger)
        {
            var configuration = options.Value;
        }

        /// <inheritdoc/>
        protected override ValueTask InitialiseAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += MessageReceivedAsync;
            Client.MessageDeleted += MessageDeletedAsync;
            Client.ReactionAdded += ReactionAddedAsync;
            Client.ReactionRemoved += ReactionRemovedAsync;
            Client.ReactionsCleared += ReactionsClearedAsync;

            return default;
        }

        private Task MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task MessageDeletedAsync(object sender, MessageDeletedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task ReactionAddedAsync(object sender, ReactionAddedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task ReactionRemovedAsync(object sender, ReactionRemovedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task ReactionsClearedAsync(object sender, ReactionsClearedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}

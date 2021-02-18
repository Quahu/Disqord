using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Rest.Repetition
{
    /// <summary>
    ///     Represents a type that intervally executes REST requests.
    /// </summary>
    public abstract class Repeater : IDisposable
    {
        /// <summary>
        ///     Gets the interval at which the requests are executed.
        /// </summary>
        public abstract TimeSpan Interval { get; }

        /// <summary>
        ///     Gets the client that will execute the requests.
        /// </summary>
        public IRestClient Client { get; }

        /// <summary>
        ///     Gets the initial request options passed.
        /// </summary>
        public IRestRequestOptions Options { get; }

        private readonly Cts _cts;

        /// <summary>
        ///     Instantiates a new <see cref="Repeater"/>.
        /// </summary>
        /// <param name="client"> The client to execute the requests with. </param>
        /// <param name="options"> The optional request options. </param>
        protected Repeater(IRestClient client, IRestRequestOptions options = null)
        {
            Client = client;
            Options = options;

            _cts = new Cts();
            _ = Task.Run(RunAsync);
        }

        /// <summary>
        ///     Finalizes this <see cref="Repeater"/>.
        /// </summary>
        ~Repeater()
        {
            Dispose();

            // This is VERY far-fetched. For removal, probably.
            Client.Logger.LogWarning("The finalizer for {0} was executed. Repeaters should be properly disposed instead.", GetType().Name);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        private async Task RunAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                await ExecuteAsync(_cts.Token).ConfigureAwait(false);
                await Task.Delay(Interval, _cts.Token).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

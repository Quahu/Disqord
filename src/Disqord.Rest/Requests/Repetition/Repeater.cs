using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;

namespace Disqord.Rest.Repetition;

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
    ///     Gets the request options passed initially to the method that created the repeater.
    /// </summary>
    public IRestRequestOptions? Options { get; }

    private readonly Cts _cts;

    /// <summary>
    ///     Instantiates a new <see cref="Repeater"/>.
    /// </summary>
    /// <param name="client"> The client to execute the requests with. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected Repeater(IRestClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Client = client;
        Options = options;

        _cts = Cts.Linked(cancellationToken);
        _ = Task.Run(RunAsync, cancellationToken);
    }

    /// <summary>
    ///     The callback method executed at the set <see cref="Interval"/>.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the repeated requests.
    /// </returns>
    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

    private async Task RunAsync()
    {
        var cancellationToken = _cts.Token;
        while (!cancellationToken.IsCancellationRequested)
        {
            await ExecuteAsync(cancellationToken).ConfigureAwait(false);
            await Task.Delay(Interval, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

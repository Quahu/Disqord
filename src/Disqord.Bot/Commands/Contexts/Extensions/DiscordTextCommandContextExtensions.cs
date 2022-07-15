using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Text;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class DiscordTextCommandContextExtensions
{
    /// <summary>
    ///     Yields this command execution flow into the background
    ///     freeing up a slot in the <see cref="ICommandQueue"/>.
    /// </summary>
    /// <example>
    ///     Yielding the command execution flow before cheap background work.
    ///     See <see cref="ContinueAsync"/> for how to re-enter the <see cref="ICommandQueue"/>.
    ///     <code>
    ///     Context.Yield();
    ///     await Task.Delay(5000); // Command code after Yield() executes in the background.
    ///     </code>
    /// </example>
    /// <returns>
    ///     <see langword="true"/> if the flow yielded.
    /// </returns>
    public static bool Yield(this IDiscordTextCommandContext context)
    {
        if (context.TryGetMetadata<Tcs>("KamajiYieldTcs", out var tcs) && tcs!.Complete())
        {
            context.SetMetadata("KamajiContinuationTcs", new Tcs());
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Asynchronously waits for this command execution flow to
    ///     be executed in the <see cref="ICommandQueue"/> again.
    /// </summary>
    /// <example>
    ///     Re-entering the <see cref="ICommandQueue"/> after <see cref="Yield"/>ing.
    ///     <code>
    ///     await Context.ContinueAsync();
    ///     // Command code executes in the queue again.
    ///     </code>
    /// </example>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the wait.
    /// </returns>
    public static async ValueTask ContinueAsync(this IDiscordTextCommandContext context)
    {
        var continuationTcs = context.GetMetadataOrDefault<Tcs>("KamajiContinuationTcs");
        if (continuationTcs == null || continuationTcs.Task.IsCompleted || !context.GetMetadata<Tcs>("KamajiYieldTcs")!.Task.IsCompleted)
            return;

        // Kamaji will treat the bath token as existing work
        // and complete the continuation TCS when there's a spot in the queue.
        context.Bot.Queue.Post(context, null);
        await continuationTcs.Task.ConfigureAwait(false);
    }

    /// <summary>
    ///     Calls <see cref="Yield"/> and returns a disposable that
    ///     calls <see cref="ContinueAsync"/> when disposed.
    ///     Handy for grouping up background logic.
    /// </summary>
    /// <example>
    ///     Yielding the command execution flow and then re-entering the <see cref="ICommandQueue"/> automatically.
    ///     See <see cref="Yield"/> and <see cref="ContinueAsync"/>.
    ///     <code>
    ///     await using (Context.BeginYield())
    ///     {
    ///         // Code to execute in the background.
    ///     }
    ///     </code>
    /// </example>
    /// <returns>
    ///     A disposable which calls <see cref="ContinueAsync"/> when disposed.
    /// </returns>
    public static YieldDisposable BeginYield(this IDiscordTextCommandContext context)
    {
        context.Yield();
        return new YieldDisposable(context);
    }

    public readonly struct YieldDisposable : IAsyncDisposable
    {
        private readonly IDiscordTextCommandContext _context;

        public YieldDisposable(IDiscordTextCommandContext context)
        {
            _context = context;
        }

        public ValueTask DisposeAsync()
        {
            return _context.ContinueAsync();
        }
    }
}

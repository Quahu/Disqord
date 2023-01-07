using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents a <see cref="PeriodicTimer"/> implementation of an <see cref="IVoiceSynchronizer"/>.
/// </summary>
public sealed class PeriodicTimerVoiceSynchronizer : TimerVoiceSynchronizer
{
    private Cts? _cts;
    private Task? _task;

    /// <summary>
    ///     Instantiates a new <see cref="PeriodicTimerVoiceSynchronizer"/>.
    /// </summary>
    /// <param name="logger"> The logger. </param>
    public PeriodicTimerVoiceSynchronizer(
        ILogger<PeriodicTimerVoiceSynchronizer> logger)
        : base(logger)
    { }

    private async Task RunTickTask()
    {
        var cancellationToken = _cts!.Token;
        using (var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(VoiceConstants.DurationMilliseconds)))
        {
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                InvokeTimerCallback();
            }
        }
    }

    /// <inheritdoc/>
    protected override void StartTicking()
    {
        _cts = new Cts();
        _task = RunTickTask();
    }

    /// <inheritdoc/>
    protected override void StopTicking()
    {
        _cts!.Cancel();
        _cts.Dispose();
        _cts = null;
        _task = null;
    }
}

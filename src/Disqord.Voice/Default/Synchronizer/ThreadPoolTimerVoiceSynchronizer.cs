using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents a <see cref="System.Threading.Timer"/> implementation of an <see cref="IVoiceSynchronizer"/>.
/// </summary>
public class ThreadPoolTimerVoiceSynchronizer : TimerVoiceSynchronizer
{
    /// <summary>
    ///     The timer instance.
    /// </summary>
    protected Timer? Timer;

    /// <summary>
    ///     Instantiates a new <see cref="ThreadPoolTimerVoiceSynchronizer"/>.
    /// </summary>
    /// <param name="logger"> The logger. </param>
    public ThreadPoolTimerVoiceSynchronizer(
        ILogger<TimerVoiceSynchronizer> logger)
        : base(logger)
    { }

    /// <inheritdoc/>
    protected override void StartTicking()
    {
        Timer = new Timer(state =>
        {
            var @this = Unsafe.As<ThreadPoolTimerVoiceSynchronizer>(state!);
            @this.InvokeTimerCallback();
        }, this, VoiceConstants.DurationMilliseconds, VoiceConstants.DurationMilliseconds);
    }

    /// <inheritdoc/>
    protected override void StopTicking()
    {
        Timer!.Dispose();
    }
}

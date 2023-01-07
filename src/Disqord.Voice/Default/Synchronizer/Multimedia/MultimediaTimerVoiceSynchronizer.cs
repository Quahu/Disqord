using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents a Windows-exclusive implementation of an <see cref="IVoiceSynchronizer"/> that offers the best timer resolution
///     possible on Windows via multimedia timers with the timer resolution being set to as high as <c>1</c> millisecond.
/// </summary>
/// <remarks>
///     This implementation has its quirks because of the high timer resolution:
///     <list type="number">
///         <item>
///             <description>
///                 All <see cref="Thread.Sleep(int)"/> calls will use the high resolution.
///                 Prior to Windows 10 2004 this new resolution would be set for the entire system rather than just the application,
///                 additionally making timers also use the high resolution, e.g. <see cref="Task.Delay(int)"/>.
///             </description>
///         </item>
///         <item>
///             <description>
///                 CPU and power usage might increase because of the more frequent interrupts.
///             </description>
///         </item>
///     </list>
///
///     Note that, for example, the Discord desktop client and most media player applications
///     also set the timer resolution to the highest possible, so it is not an uncommon practice
///     when dealing with audio playback on Windows.
///     <para/>
///     Windows limits the amount of multimedia timers to <c>16</c> per-process.
///     This means that if, for some reason, your application has already created this many multimedia timers,
///     the synchronizer will fall back onto the <see cref="ThreadPoolTimerVoiceSynchronizer"/> implementation.
/// </remarks>
[SupportedOSPlatform("windows")]
public sealed partial class MultimediaTimerVoiceSynchronizer : ThreadPoolTimerVoiceSynchronizer
{
    private uint _timerId;
    private GCHandle _gcHandle;

    /// <summary>
    ///     Instantiates a new <see cref="MultimediaTimerVoiceSynchronizer"/>.
    /// </summary>
    /// <param name="options"> The options. </param>
    /// <param name="logger"> The logger. </param>
    public MultimediaTimerVoiceSynchronizer(
        IOptions<MultimediaTimerVoiceUdpSynchronizerConfiguration> options,
        ILogger<MultimediaTimerVoiceSynchronizer> logger)
        : base(logger)
    {
        var configuration = options.Value;
        Setup(configuration, logger);
    }

    /// <inheritdoc/>
    protected override unsafe void StartTicking()
    {
        _gcHandle = GCHandle.Alloc(this, GCHandleType.Weak);
        timeBeginPeriod(_resolution);
        _timerId = (uint) timeSetEvent(VoiceConstants.DurationMilliseconds, _resolution, &TimerCallback, GCHandle.ToIntPtr(_gcHandle),
            FUEVENT.TIME_PERIODIC | FUEVENT.TIME_KILL_SYNCHRONOUS);

        if (_timerId == 0)
        {
            Logger.LogWarning("Failed to create the multimedia timer - falling back onto the base implementation.");

            _gcHandle.Free();
            timeEndPeriod(_resolution);
            base.StartTicking();
        }
    }

    /// <inheritdoc/>
    protected override void StopTicking()
    {
        if (_timerId != 0)
        {
            timeKillEvent(_timerId);
            timeEndPeriod(_resolution);
            _gcHandle.Free();
        }
        else
        {
            base.StopTicking();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall), typeof(CallConvSuppressGCTransition) })]
    private static void TimerCallback(uint uTimerID, uint uMsg, IntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
    {
        try
        {
            var @this = Unsafe.As<MultimediaTimerVoiceSynchronizer>(GCHandle.FromIntPtr(dwUser).Target!);
            if (uTimerID == 0)
                return;

            @this.InvokeTimerCallback();
        }
        catch { }
    }
}

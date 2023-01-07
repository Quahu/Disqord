using System;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents a <see cref="System.Threading.Timer"/> implementation of an <see cref="IVoiceSynchronizer"/>.
/// </summary>
public abstract class TimerVoiceSynchronizer : VoiceSynchronizer
{
    /// <summary>
    ///     Instantiates a new <see cref="TimerVoiceSynchronizer"/>.
    /// </summary>
    /// <param name="logger"> The logger. </param>
    protected TimerVoiceSynchronizer(
        ILogger<TimerVoiceSynchronizer> logger)
        : base(logger)
    { }

    /// <summary>
    ///     Invokes <see cref="OnTimerCallback"/>, catches and logs any exceptions that might occur.
    /// </summary>
    protected void InvokeTimerCallback()
    {
        try
        {
            OnTimerCallback();
        }
        catch (Exception ex)
        {
            try
            {
                Logger.LogError(ex, "An exception occurred while executing the timer callback.");
            }
            catch { }
        }
    }

    /// <summary>
    ///     Invoked when the timer callback fires.
    /// </summary>
    protected virtual void OnTimerCallback()
    {
        OnTick();
    }
}

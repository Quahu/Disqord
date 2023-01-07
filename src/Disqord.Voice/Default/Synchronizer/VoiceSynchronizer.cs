using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the base implementation of an <see cref="IVoiceSynchronizer"/>.
/// </summary>
public abstract class VoiceSynchronizer : IVoiceSynchronizer
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    private ImmutableArray<IVoiceUdpClient> _clients = ImmutableArray<IVoiceUdpClient>.Empty;

    private int _tickState;
    private const int NotTickingState = 0;
    private const int TickingState = 1;

    // private const long TicksPerMillisecond = 10000;
    // private const long TicksPerSecond = TicksPerMillisecond * 1000;
    // private static readonly double TickFrequency = (double) TicksPerSecond / Stopwatch.Frequency;
    //
    // private long? _nextTickTimestamp;
    // private const long DevToleranceMilliseconds = 1;

    /// <summary>
    ///     Instantiates a new <see cref="VoiceSynchronizer"/>.
    /// </summary>
    /// <param name="logger"> The logger. </param>
    protected VoiceSynchronizer(
        ILogger<VoiceSynchronizer> logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     Starts the ticking mechanism of this synchronizer.
    /// </summary>
    /// <remarks>
    ///     This method holds the lock on <see langword="this"/> instance.
    /// </remarks>
    protected abstract void StartTicking();

    /// <summary>
    ///     Stops the ticking mechanism of this synchronizer.
    /// </summary>
    /// <remarks>
    ///     This method holds the lock on <see langword="this"/> instance.
    /// </remarks>
    protected abstract void StopTicking();

    /// <inheritdoc/>
    public void Subscribe(IVoiceUdpClient client)
    {
        lock (this)
        {
            _clients = _clients.Add(client);

            if (_tickState == TickingState)
                return;

            StartTicking();
            _tickState = TickingState;
        }
    }

    /// <inheritdoc/>
    public void Unsubscribe(IVoiceUdpClient client)
    {
        lock (this)
        {
            _clients = _clients.Remove(client);

            if (_tickState == NotTickingState || !_clients.IsEmpty)
                return;

            StopTicking();
            // _nextTickTimestamp = null;
            _tickState = NotTickingState;
        }
    }

    protected void OnTick()
    {
        // var nextTickTimestamp = _nextTickTimestamp;
        // if (nextTickTimestamp != null)
        // {
        //     var delta = TimeSpan.FromTicks(unchecked((long) ((nextTickTimestamp.Value - Stopwatch.GetTimestamp()) * TickFrequency)));
        //     var deltaMilliseconds = Math.Round(delta.TotalMilliseconds, MidpointRounding.AwayFromZero);
        //     if (delta > TimeSpan.Zero)
        //     {
        //         if (deltaMilliseconds > VoiceUtilities.DurationMilliseconds + DevToleranceMilliseconds)
        //         {
        //             // Logger.LogTrace("Synchronizer ticked too fast ({DeltaMilliseconds}ms). Delta is {Delta}", deltaMilliseconds - VoiceUtilities.DurationMilliseconds, delta);
        //             _nextTickTimestamp = Stopwatch.GetTimestamp();
        //         }
        //     }
        //     else if (deltaMilliseconds < -DevToleranceMilliseconds)
        //     {
        //         // Logger.LogTrace("Synchronizer ticked too slow ({DeltaMilliseconds}ms). Delta is {Delta}", -deltaMilliseconds, delta);
        //         _nextTickTimestamp = Stopwatch.GetTimestamp();
        //     }
        //
        //     _nextTickTimestamp += unchecked((long) (VoiceUtilities.DurationMilliseconds * TicksPerMillisecond / TickFrequency));
        // }
        // else
        // {
        //     _nextTickTimestamp = Stopwatch.GetTimestamp();
        // }

        var clients = _clients;
        foreach (var client in clients)
        {
            client.OnSynchronizerTick();
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Disqord.Voice;
using Qommon.Threading;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents the base implementation of an audio player.
/// </summary>
public class AudioPlayer : IAsyncDisposable
{
    /// <summary>
    ///     Gets the connection of this audio player.
    /// </summary>
    public IVoiceConnection Connection { get; }

    /// <summary>
    ///     Gets the ID of the guild this audio player is connected to.
    /// </summary>
    public Snowflake GuildId => Connection.GuildId;

    /// <summary>
    ///     Gets the ID of the channel this audio player is connected to.
    /// </summary>
    public Snowflake ChannelId => Connection.ChannelId;

    /// <summary>
    ///     Gets or sets the audio source currently being played by this audio player.
    /// </summary>
    public AudioSource? Source
    {
        get
        {
            var source = Volatile.Read(ref _source);
            return source.Task.IsCompletedSuccessfully
                ? source.Task.Result
                : null;
        }
        set
        {
            ThrowIfDisposed();

            lock (_sourceLock)
            {
                var oldSource = _source.Task.IsCompletedSuccessfully
                    ? _source.Task.Result
                    : null;

                if (oldSource == value)
                    return;

                _sourceCts.Cancel();
                _sourceCts.Dispose();
                _sourceCts = new();

                if (oldSource != null)
                {
                    _source = new();
                }

                if (value != null)
                {
                    _source.SetResult(value);
                }
            }
        }
    }

    /// <summary>
    ///     Gets whether this audio player is started.
    /// </summary>
    public bool IsStarted => _stopCts != null;

    /// <summary>
    ///     Gets whether this audio player is playing an audio source.
    /// </summary>
    public bool IsPlaying => Source != null;

    /// <summary>
    ///     Gets whether this audio player is paused.
    /// </summary>
    public bool IsPaused => !_pauseAmre.IsSet;

    /// <summary>
    ///     Gets or sets whether the bot should be a priority speaker,
    ///     i.e. whether it should lower the volume of other users in the voice channel
    ///     while playing audio.
    /// </summary>
    public bool IsPrioritySpeaker { get; set; }

    /// <summary>
    ///     Gets the speaking flags that should be used by the bot
    ///     when sending audio.
    /// </summary>
    protected virtual SpeakingFlags SpeakingFlags
    {
        get
        {
            var flags = SpeakingFlags.Microphone;
            if (IsPrioritySpeaker)
                flags |= SpeakingFlags.Priority;

            return flags;
        }
    }

    private volatile Cts? _stopCts;

    private readonly object _sourceLock = new();
    private Cts _sourceCts;
    private TaskCompletionSource<AudioSource> _source;

    private readonly AsyncManualResetEvent _pauseAmre;
    private readonly object _stopLock = new();
    private bool _isDisposed;

    public AudioPlayer(IVoiceConnection connection)
    {
        Connection = connection;

        _sourceCts = new();
        _source = new();
        _pauseAmre = new(true);
    }

    protected void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().Name);
    }

    /// <summary>
    ///     Invoked when an <see cref="AudioSource"/> has started playing.
    /// </summary>
    /// <param name="source"> The <see cref="AudioSource"/> that has started playing. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnSourceStarted(AudioSource source)
    {
        return default;
    }

    /// <summary>
    ///     Invoked when an <see cref="AudioSource"/> has finished playing.
    /// </summary>
    /// <param name="source"> The <see cref="AudioSource"/> that has finished playing. </param>
    /// <param name="forced"> <see langword="true"/> if the previous audio source was replaced with a new one. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnSourceFinished(AudioSource source, bool forced)
    {
        return default;
    }

    /// <summary>
    ///     Invoked when this audio player is paused.
    /// </summary>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnPaused()
    {
        return default;
    }

    /// <summary>
    ///     Invoked when this audio player is resumed.
    /// </summary>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnResumed()
    {
        return default;
    }

    /// <summary>
    ///     Invoked when this audio player is started.
    /// </summary>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnStarted()
    {
        return default;
    }

    /// <summary>
    ///     Invoked when this audio player is stopped.
    /// </summary>
    /// <param name="exception"> The exception that caused the stop or <see langword="null"/> if no exception occurred. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnStopped(Exception? exception)
    {
        return default;
    }

    /// <summary>
    ///     Tries to set the audio source of this audio player
    ///     if one is not already set.
    /// </summary>
    /// <param name="source"> The audio source to set. </param>
    /// <returns>
    ///     <see langword="true"/> if the source was set.
    /// </returns>
    public bool TrySetSource(AudioSource source)
    {
        ThrowIfDisposed();

        lock (_sourceLock)
        {
            if (_source.Task.IsCompletedSuccessfully)
                return false;

            _source.SetResult(source);
            return true;
        }
    }

    /// <summary>
    ///     Pauses this audio player.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> if already paused.
    /// </returns>
    public bool Pause()
    {
        ThrowIfDisposed();

        return _pauseAmre.Reset();
    }

    /// <summary>
    ///     Resumes this audio player.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> if already resumed.
    /// </returns>
    public bool Resume()
    {
        ThrowIfDisposed();

        return _pauseAmre.Set();
    }

    /// <summary>
    ///     Starts this audio player.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> if already started.
    /// </returns>
    public bool Start()
    {
        ThrowIfDisposed();

        lock (_stopLock)
        {
            if (_stopCts == null)
            {
                _stopCts = new();
                _ = ExecuteAsync();
                return true;
            }

            return false;
        }
    }

    /// <summary>
    ///     Stops this audio player.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> if already stopped.
    /// </returns>
    public bool Stop()
    {
        ThrowIfDisposed();

        lock (_stopLock)
        {
            if (_stopCts != null)
            {
                _stopCts.Cancel();
                _stopCts.Dispose();
                _stopCts = null;
                return true;
            }

            return false;
        }
    }

    /// <summary>
    ///     Moves this audio player to a different channel within the guild.
    /// </summary>
    /// <param name="channelId"> The ID of the channel to move to. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    public ValueTask SetChannelIdAsync(Snowflake channelId, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return Connection.SetChannelIdAsync(channelId, cancellationToken);
    }

    private async Task WaitIfPausedAsync(CancellationToken cancellationToken)
    {
        var connection = Connection;
        var pauseTask = _pauseAmre.WaitAsync(cancellationToken);
        if (!pauseTask.IsCompleted)
        {
            await connection.SetSpeakingFlagsAsync(SpeakingFlags.None, cancellationToken).ConfigureAwait(false);
            await OnPaused().ConfigureAwait(false);

            for (var i = 0; i < 5; i++)
            {
                await connection.SendPacketAsync(VoiceConstants.SilencePacket, cancellationToken).ConfigureAwait(false);
            }

            await pauseTask.ConfigureAwait(false);

            await OnResumed().ConfigureAwait(false);
            await connection.SetSpeakingFlagsAsync(SpeakingFlags, cancellationToken).ConfigureAwait(false);
        }
    }

    private protected async Task ExecuteAsync()
    {
        await Task.Yield();

        Cts? stopCts;
        lock (_stopLock)
        {
            stopCts = _stopCts;
        }

        if (stopCts == null || stopCts.IsCancellationRequested)
            return;

        var stopCancellationToken = stopCts.Token;
        Exception? exception = null;
        var connection = Connection;
        try
        {
            await OnStarted().ConfigureAwait(false);

            await connection.WaitUntilReadyAsync(stopCancellationToken).ConfigureAwait(false);
            while (!stopCancellationToken.IsCancellationRequested)
            {
                Task<AudioSource> sourceTask;
                CancellationToken sourceCancellationToken;
                lock (_sourceLock)
                {
                    sourceTask = _source.Task;
                    sourceCancellationToken = _sourceCts.Token;
                }

                var source = await sourceTask.ConfigureAwait(false);
                await connection.SetSpeakingFlagsAsync(SpeakingFlags, stopCancellationToken).ConfigureAwait(false);

                await OnSourceStarted(source).ConfigureAwait(false);

                using (var linkedCts = Cts.Linked(stopCancellationToken, sourceCancellationToken))
                {
                    var linkedCancellationToken = linkedCts.Token;
                    var enumerator = source.GetAsyncEnumerator(linkedCancellationToken);
                    try
                    {
                        while (!linkedCancellationToken.IsCancellationRequested && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await WaitIfPausedAsync(linkedCancellationToken).ConfigureAwait(false);

                            await connection.SendPacketAsync(enumerator.Current, linkedCancellationToken).ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        await enumerator.DisposeAsync().ConfigureAwait(false);
                    }

                    lock (_sourceLock)
                    {
                        _source = new();
                    }

                    if (!stopCts.IsCancellationRequested)
                    {
                        await OnSourceFinished(source, sourceCancellationToken.IsCancellationRequested).ConfigureAwait(false);
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            for (var i = 0; i < 5; i++)
            {
                await connection.SendPacketAsync(VoiceConstants.SilencePacket, stopCancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        finally
        {
            Stop();
            await OnStopped(exception).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return default;

        _isDisposed = true;

        lock (_stopLock)
        {
            if (_stopCts != null)
            {
                _stopCts.Cancel();
                _stopCts.Dispose();
                _stopCts = null;
            }

            lock (_sourceLock)
            {
                _sourceCts.Cancel();
                _sourceCts.Dispose();
            }
        }

        return default;
    }
}

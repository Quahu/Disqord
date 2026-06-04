using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Voice;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents a subscription to a single user's audio receive stream.
///     Packets are delivered as an async enumerable of received voice packets.
///     The consumer owns each yielded packet and must dispose it after use.
/// </summary>
public class AudioReceiverSubscription : IAsyncEnumerable<VoiceReceivePacket>, IAsyncDisposable
{
    /// <summary>
    ///     Gets the ID of the user this subscription is receiving audio from.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the options used to configure this subscription.
    /// </summary>
    public AudioReceiveSubscriptionOptions Options { get; }

    private readonly Channel<VoiceReceivePacket> _channel;
    private readonly object _timerLock = new();
    private Timer? _endTimer;
    private int _isCompleted;

    internal event Action<AudioReceiverSubscription>? Closed;

    internal AudioReceiverSubscription(Snowflake userId, AudioReceiveSubscriptionOptions options)
    {
        UserId = userId;
        Options = options;
        var maxBufferedDuration = options.MaxBufferedDuration;
        if (maxBufferedDuration <= TimeSpan.Zero)
        {
            _channel = Channel.CreateUnbounded<VoiceReceivePacket>();
        }
        else
        {
            var capacity = Math.Max(1, (int) Math.Ceiling(maxBufferedDuration.TotalMilliseconds / VoiceConstants.DurationMilliseconds));
            var fullMode = options.BufferFullMode switch
            {
                AudioReceiveBufferFullMode.Wait => BoundedChannelFullMode.Wait,
                AudioReceiveBufferFullMode.DropNewest => BoundedChannelFullMode.DropWrite,
                _ => BoundedChannelFullMode.DropOldest
            };

            var channelOptions = new BoundedChannelOptions(capacity)
            {
                FullMode = fullMode,
                SingleWriter = true,
            };

            _channel = fullMode == BoundedChannelFullMode.DropOldest
                ? Channel.CreateBounded<VoiceReceivePacket>(channelOptions, static (packet) => packet.Dispose())
                : Channel.CreateBounded<VoiceReceivePacket>(channelOptions);
        }
    }

    internal ValueTask<bool> WriteAsync(VoiceReceivePacket packet)
    {
        if (Volatile.Read(ref _isCompleted) != 0)
            return new ValueTask<bool>(false);

        var opus = packet.Opus;
        if (Options.EndBehaviorType == AudioReceiveEndBehaviorType.AfterInactivity
            || (Options.EndBehaviorType == AudioReceiveEndBehaviorType.AfterSilence
                && (!opus.Span.SequenceEqual(VoiceConstants.SilencePacket.Span) || _endTimer == null)))
        {
            RenewEndTimer();
        }

        if (_channel.Writer.TryWrite(packet))
            return new ValueTask<bool>(true);

        if (Options.BufferFullMode != AudioReceiveBufferFullMode.Wait)
            return new ValueTask<bool>(false);

        return WriteSlowAsync(packet);
    }

    private async ValueTask<bool> WriteSlowAsync(VoiceReceivePacket packet)
    {
        while (await _channel.Writer.WaitToWriteAsync().ConfigureAwait(false))
        {
            if (Volatile.Read(ref _isCompleted) != 0)
                return false;

            if (_channel.Writer.TryWrite(packet))
                return true;
        }

        return false;
    }

    private void RenewEndTimer()
    {
        if (Options.EndBehaviorType == AudioReceiveEndBehaviorType.Manual)
            return;

        if (Options.EndBehaviorDuration <= TimeSpan.Zero)
        {
            Complete();
            return;
        }

        lock (_timerLock)
        {
            if (_endTimer != null)
            {
                _endTimer.Change(Options.EndBehaviorDuration, Timeout.InfiniteTimeSpan);
            }
            else
            {
                _endTimer = new Timer(static state => ((AudioReceiverSubscription) state!).Complete(),
                    this, Options.EndBehaviorDuration, Timeout.InfiniteTimeSpan);
            }
        }
    }

    internal void Complete()
    {
        if (Interlocked.Exchange(ref _isCompleted, 1) != 0)
            return;

        lock (_timerLock)
        {
            _endTimer?.Dispose();
            _endTimer = null;
        }

        _channel.Writer.TryComplete();
        Closed?.Invoke(this);
    }

    /// <inheritdoc/>
    public IAsyncEnumerator<VoiceReceivePacket> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new PacketEnumerator(_channel.Reader, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        Complete();

        // Drain and dispose any remaining pooled packets in the channel.
        while (_channel.Reader.TryRead(out var packet))
        {
            packet.Dispose();
        }

        return default;
    }

    private sealed class PacketEnumerator : IAsyncEnumerator<VoiceReceivePacket>
    {
        private readonly ChannelReader<VoiceReceivePacket> _reader;
        private readonly CancellationToken _cancellationToken;
        private VoiceReceivePacket _current;
        private bool _hasCurrent;

        public PacketEnumerator(ChannelReader<VoiceReceivePacket> reader, CancellationToken cancellationToken)
        {
            _reader = reader;
            _cancellationToken = cancellationToken;
        }

        public VoiceReceivePacket Current => _current;

        public async ValueTask<bool> MoveNextAsync()
        {
            if (_hasCurrent)
            {
                _current = default;
                _hasCurrent = false;
            }

            while (await _reader.WaitToReadAsync(_cancellationToken).ConfigureAwait(false))
            {
                if (_reader.TryRead(out _current))
                {
                    _hasCurrent = true;
                    return true;
                }
            }

            return false;
        }

        public ValueTask DisposeAsync()
        {
            if (_hasCurrent)
            {
                _current.Dispose();
            }

            _current = default;
            _hasCurrent = false;
            return default;
        }
    }
}

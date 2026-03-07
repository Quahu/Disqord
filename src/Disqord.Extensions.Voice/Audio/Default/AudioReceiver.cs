using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Voice;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Manages per-user audio receive subscriptions for a voice connection.
///     Handles packet routing, silence gap synthesis, and subscription lifecycle.
/// </summary>
public class AudioReceiver : IAsyncDisposable
{
    /// <summary>
    ///     Gets the underlying voice connection.
    /// </summary>
    public IVoiceConnection Connection { get; }

    private readonly object _subscriptionLock = new();
    private readonly Dictionary<Snowflake, SubscriptionState> _subscriptions = [];
    private bool _isListening;
    private bool _isDisposed;

    private sealed class SubscriptionState(AudioReceiverSubscription subscription)
    {
        public AudioReceiverSubscription Subscription { get; } = subscription;

        public uint LastSsrc { get; set; }

        public ushort LastSequence { get; set; }

        public bool HasLastSequence { get; set; }
    }

    public AudioReceiver(IVoiceConnection connection)
    {
        Connection = connection;
        connection.UserDisconnected += OnUserDisconnected;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    private void OnUserDisconnected(Snowflake userId)
    {
        lock (_subscriptionLock)
        {
            if (_subscriptions.TryGetValue(userId, out var state))
            {
                state.Subscription.Complete();
            }
        }
    }

    /// <summary>
    ///     Listens for users connecting to the voice session and yields subscriptions based on the provided predicate.
    ///     When a user connects, the predicate is called with their ID. If it returns options, a subscription is created and yielded.
    ///     Subscriptions are automatically completed when the user disconnects.
    /// </summary>
    /// <param name="userPredicate"> A predicate that returns subscription options for users to listen to, or <see langword="null"/> to skip. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. Cancelling stops listening for new subscriptions. </param>
    /// <returns>
    ///     An async enumerable of subscriptions for connecting users.
    /// </returns>
    public async IAsyncEnumerable<AudioReceiverSubscription> ListenAsync(
        Func<Snowflake, AudioReceiveSubscriptionOptions?> userPredicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        lock (_subscriptionLock)
        {
            if (_isListening)
            {
                throw new InvalidOperationException("ListenAsync is already active.");
            }

            _isListening = true;
        }

        var newUserChannel = Channel.CreateUnbounded<Snowflake>(new UnboundedChannelOptions { SingleReader = true });

        Connection.UserConnected += OnUserConnected;
        await Connection.SetPacketSinkAsync(OnPacketReceivedAsync, cancellationToken).ConfigureAwait(false);

        try
        {
            // Snapshot currently connected users. Any user that connected between
            // hooking the event and this read is either in the snapshot or in the channel (or both).
            foreach (var userId in Connection.ConnectedUserIds)
            {
                var subscription = TryCreateListenSubscription(userId, userPredicate);
                if (subscription != null)
                {
                    yield return subscription;
                }
            }

            await foreach (var userId in newUserChannel.Reader.ReadAllAsync(cancellationToken).ConfigureAwait(false))
            {
                var subscription = TryCreateListenSubscription(userId, userPredicate);
                if (subscription != null)
                {
                    yield return subscription;
                }
            }
        }
        finally
        {
            Connection.UserConnected -= OnUserConnected;

            lock (_subscriptionLock)
            {
                _isListening = false;
            }
        }

        yield break;

        void OnUserConnected(Snowflake userId)
        {
            newUserChannel.Writer.TryWrite(userId);
        }
    }

    private AudioReceiverSubscription? TryCreateListenSubscription(Snowflake userId, Func<Snowflake, AudioReceiveSubscriptionOptions?> userPredicate)
    {
        lock (_subscriptionLock)
        {
            if (_subscriptions.ContainsKey(userId))
            {
                return null;
            }
        }

        var options = userPredicate(userId);
        if (options == null)
        {
            return null;
        }

        lock (_subscriptionLock)
        {
            if (_subscriptions.ContainsKey(userId))
            {
                return null;
            }

            var subscription = new AudioReceiverSubscription(userId, options);
            subscription.Closed += OnSubscriptionClosed;
            _subscriptions[userId] = new SubscriptionState(subscription);
            return subscription;
        }
    }

    /// <summary>
    ///     Subscribes to audio packets from the specified user.
    ///     If a subscription already exists for this user, the existing subscription is returned.
    ///     Automatically enables receiving on the voice connection when the first subscription is created.
    /// </summary>
    /// <param name="userId"> The ID of the user to subscribe to. </param>
    /// <param name="options"> The subscription options, or <see langword="null"/> to use defaults. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns> The audio receive subscription for the user. </returns>
    public async ValueTask<AudioReceiverSubscription> SubscribeAsync(Snowflake userId, AudioReceiveSubscriptionOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        bool shouldEnable;
        AudioReceiverSubscription subscription;
        lock (_subscriptionLock)
        {
            if (_subscriptions.TryGetValue(userId, out var existingState))
            {
                return existingState.Subscription;
            }

            subscription = new AudioReceiverSubscription(userId, options ?? new AudioReceiveSubscriptionOptions());
            subscription.Closed += OnSubscriptionClosed;
            _subscriptions[userId] = new SubscriptionState(subscription);
            shouldEnable = _subscriptions.Count == 1;
        }

        if (shouldEnable)
        {
            await Connection.SetPacketSinkAsync(OnPacketReceivedAsync, cancellationToken).ConfigureAwait(false);
        }

        return subscription;
    }

    /// <summary>
    ///     Unsubscribes from audio packets for the specified user and disposes the subscription.
    /// </summary>
    /// <param name="userId"> The ID of the user to unsubscribe from. </param>
    /// <returns> <see langword="true"/> if a subscription was found and removed; otherwise, <see langword="false"/>. </returns>
    public async ValueTask<bool> UnsubscribeAsync(Snowflake userId)
    {
        AudioReceiverSubscription? subscription;
        lock (_subscriptionLock)
        {
            if (!_subscriptions.Remove(userId, out var state))
            {
                return false;
            }

            subscription = state.Subscription;
            subscription.Closed -= OnSubscriptionClosed;
        }

        await subscription.DisposeAsync().ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Gets a snapshot of all active subscriptions keyed by user ID.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, AudioReceiverSubscription> GetSubscriptions()
    {
        lock (_subscriptionLock)
        {
            return _subscriptions.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value.Subscription);
        }
    }

    private void OnSubscriptionClosed(AudioReceiverSubscription subscription)
    {
        lock (_subscriptionLock)
        {
            if (_subscriptions.TryGetValue(subscription.UserId, out var state)
                && state.Subscription == subscription)
            {
                _subscriptions.Remove(subscription.UserId);
            }
        }
    }

    private async ValueTask OnPacketReceivedAsync(VoiceReceivePacket packet)
    {
        if (packet.UserId == null)
        {
            packet.Dispose();
            return;
        }

        SubscriptionState? state;
        lock (_subscriptionLock)
        {
            _subscriptions.TryGetValue(packet.UserId.Value, out state);
        }

        if (state == null)
        {
            packet.Dispose();
            return;
        }

        // When a user reconnects, they get a new SSRC with fresh sequence numbers.
        // Reset tracking so we don't misinterpret the new stream as out-of-order or a huge gap.
        if (state.HasLastSequence && packet.Ssrc != state.LastSsrc)
        {
            state.HasLastSequence = false;
        }

        if (state.HasLastSequence)
        {
            var sequenceDelta = (ushort) (packet.Sequence - state.LastSequence);

            // Duplicate packet.
            if (sequenceDelta == 0)
            {
                packet.Dispose();
                return;
            }

            // Out-of-order / late packet.
            if (sequenceDelta >= 0x8000)
            {
                packet.Dispose();
                return;
            }
        }

        state.LastSsrc = packet.Ssrc;
        state.LastSequence = packet.Sequence;
        state.HasLastSequence = true;

        if (!await state.Subscription.WriteAsync(packet).ConfigureAwait(false))
        {
            packet.Dispose();
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        Connection.UserDisconnected -= OnUserDisconnected;
        await Connection.SetPacketSinkAsync(null).ConfigureAwait(false);

        AudioReceiverSubscription[] subscriptions;
        lock (_subscriptionLock)
        {
            subscriptions = _subscriptions.Values.Select(static x => x.Subscription).ToArray();
            _subscriptions.Clear();
        }

        foreach (var subscription in subscriptions)
        {
            subscription.Closed -= OnSubscriptionClosed;
            await subscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}

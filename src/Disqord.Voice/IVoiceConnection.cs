using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Voice.Api;

namespace Disqord.Voice;

public delegate ValueTask SetVoiceStateDelegate(Snowflake guildId, Snowflake? channelId, CancellationToken cancellationToken);

/// <summary>
///     Represents the delegate for handling received voice packets.
///     The delegate takes ownership of the packet and is responsible for disposing it.
/// </summary>
/// <param name="packet"> The received voice packet. </param>
public delegate ValueTask VoicePacketSinkDelegate(VoiceReceivePacket packet);

/// <summary>
///     Represents the delegate invoked when a user connects to or disconnects from a voice session.
/// </summary>
/// <param name="userId"> The ID of the user. </param>
public delegate void VoiceUserPresenceDelegate(Snowflake userId);

/// <summary>
///     Represents a voice connection to a Discord voice channel.
/// </summary>
public interface IVoiceConnection : IAsyncDisposable
{
    /// <summary>
    ///     Gets the ID of the guild this connection belongs to.
    /// </summary>
    Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the voice channel this connection is attached to.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the current member (bot user) on this connection.
    /// </summary>
    Snowflake CurrentMemberId { get; }

    /// <summary>
    ///     Gets a snapshot of the currently connected user IDs in this voice session.
    /// </summary>
    IReadOnlySet<Snowflake> ConnectedUserIds { get; }

    /// <summary>
    ///     Occurs when a user connects to this voice session.
    ///     Fired after <see cref="ConnectedUserIds"/> has been updated.
    /// </summary>
    event VoiceUserPresenceDelegate? UserConnected;

    /// <summary>
    ///     Occurs when a user disconnects from this voice session.
    ///     Fired after <see cref="ConnectedUserIds"/> has been updated.
    /// </summary>
    event VoiceUserPresenceDelegate? UserDisconnected;

    /// <summary>
    ///     Moves this connection to a different voice channel within the same guild.
    /// </summary>
    /// <param name="channelId"> The ID of the target voice channel. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    ValueTask SetChannelIdAsync(Snowflake channelId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets the speaking flags for this connection.
    /// </summary>
    /// <param name="flags"> The speaking flags to set. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    ValueTask SetSpeakingFlagsAsync(SpeakingFlags flags, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends an Opus-encoded audio packet over the voice connection.
    /// </summary>
    /// <param name="opus"> The Opus-encoded audio data to send. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    ValueTask SendPacketAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets the delegate that receives voice packets, enabling or disabling voice packet receiving.
    ///     The sink takes ownership of each packet and is responsible for calling <see cref="VoiceReceivePacket.Dispose"/>.
    ///     Pass <see langword="null"/> to disable receiving.
    /// </summary>
    /// <param name="sink"> The delegate to receive voice packets, or <see langword="null"/> to disable receiving. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    ValueTask SetPacketSinkAsync(VoicePacketSinkDelegate? sink, CancellationToken cancellationToken = default);
}

/// <summary>
///     Extends <see cref="IVoiceConnection"/> with members for managing
///     the connection lifecycle and accessing low-level components.
/// </summary>
public interface IVoiceConnectionHost : IVoiceConnection
{
    /// <summary>
    ///     Gets the voice gateway client for this connection.
    /// </summary>
    IVoiceGatewayClient Gateway { get; }

    /// <summary>
    ///     Gets the voice UDP client for this connection.
    /// </summary>
    IVoiceUdpClient Udp { get; }

    /// <summary>
    ///     Gets the voice synchronizer responsible for timing audio packets.
    /// </summary>
    IVoiceSynchronizer Synchronizer { get; }

    /// <summary>
    ///     Waits until this voice connection is ready to send and receive audio.
    ///     Resets during reconnection, allowing callers to await readiness again after a connection drop.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    Task WaitUntilReadyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Notifies this connection of a voice state update from the gateway.
    /// </summary>
    /// <param name="channelId"> The channel ID from the voice state, or <see langword="null"/> if disconnected. </param>
    /// <param name="sessionId"> The session ID from the voice state. </param>
    void OnVoiceStateUpdate(Snowflake? channelId, string sessionId);

    /// <summary>
    ///     Notifies this connection of a voice server update from the gateway.
    /// </summary>
    /// <param name="token"> The voice server token. </param>
    /// <param name="endpoint"> The voice server endpoint, or <see langword="null"/> if the server is being reallocated. </param>
    void OnVoiceServerUpdate(string token, string? endpoint);

    /// <summary>
    ///     Runs the voice connection lifecycle, including connecting, reconnecting, and handling gateway events.
    /// </summary>
    /// <param name="stoppingToken"> The cancellation token that signals the connection should stop. </param>
    Task RunAsync(CancellationToken stoppingToken);
}

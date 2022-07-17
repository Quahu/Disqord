using System;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway;

/// <summary>
///     Represents the voice state of a member.
/// </summary>
public interface IVoiceState : IGuildEntity, IGatewayClientEntity, IJsonUpdatable<VoiceStateJsonModel>
{
    /// <summary>
    ///     Gets the ID of the member this voice state is for.
    /// </summary>
    Snowflake MemberId { get; }

    /// <summary>
    ///     Gets the ID of the voice channel the member is connected to.
    /// </summary>
    /// <returns>
    ///     The ID of the channel or <see langword="null"/> if the member left the voice channel.
    /// </returns>
    Snowflake? ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the voice session.
    /// </summary>
    string SessionId { get; }

    /// <summary>
    ///     Gets whether the member is deafened in the guild.
    /// </summary>
    bool IsDeafened { get; }

    /// <summary>
    ///     Gets whether the member is muted in the guild.
    /// </summary>
    bool IsMuted { get; }

    /// <summary>
    ///     Gets whether the member deafened themselves.
    /// </summary>
    bool IsSelfDeafened { get; }

    /// <summary>
    ///     Gets whether the member muted themselves.
    /// </summary>
    bool IsSelfMuted { get; }

    /// <summary>
    ///     Gets whether the member is streaming.
    /// </summary>
    bool IsStreaming { get; }

    /// <summary>
    ///     Gets whether the member is transmitting camera feed.
    /// </summary>
    bool IsTransmittingVideo { get; }

    /// <summary>
    ///     Gets when the member requested to speak.
    /// </summary>
    DateTimeOffset? RequestedToSpeakAt { get; }
}

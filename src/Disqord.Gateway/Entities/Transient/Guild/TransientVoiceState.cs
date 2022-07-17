using System;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientVoiceState : TransientGatewayClientEntity<VoiceStateJsonModel>, IVoiceState
{
    /// <inheritdoc/>
    public Snowflake GuildId => Model.GuildId.Value;

    /// <inheritdoc/>
    public Snowflake? ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public Snowflake MemberId => Model.UserId;

    /// <inheritdoc/>
    public string SessionId => Model.SessionId;

    /// <inheritdoc/>
    public bool IsDeafened => Model.Deaf;

    /// <inheritdoc/>
    public bool IsMuted => Model.Mute;

    /// <inheritdoc/>
    public bool IsSelfDeafened => Model.SelfDeaf;

    /// <inheritdoc/>
    public bool IsSelfMuted => Model.SelfMute;

    /// <inheritdoc/>
    public bool IsStreaming => Model.SelfStream.GetValueOrDefault();

    /// <inheritdoc/>
    public bool IsTransmittingVideo => Model.SelfVideo;

    /// <inheritdoc/>
    public DateTimeOffset? RequestedToSpeakAt => Model.RequestToSpeakTimestamp;

    public TransientVoiceState(IClient client, VoiceStateJsonModel model)
        : base(client, model)
    { }
}
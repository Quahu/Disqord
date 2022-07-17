using System;
using System.ComponentModel;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class CachedVoiceState : CachedEntity, IVoiceState
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public Snowflake MemberId { get; }

    /// <inheritdoc/>
    public Snowflake? ChannelId { get; private set; }

    /// <inheritdoc/>
    public string SessionId { get; private set; } = null!;

    /// <inheritdoc/>
    public bool IsDeafened { get; private set; }

    /// <inheritdoc/>
    public bool IsMuted { get; private set; }

    /// <inheritdoc/>
    public bool IsSelfDeafened { get; private set; }

    /// <inheritdoc/>
    public bool IsSelfMuted { get; private set; }

    /// <inheritdoc/>
    public bool IsStreaming { get; private set; }

    /// <inheritdoc/>
    public bool IsTransmittingVideo { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? RequestedToSpeakAt { get; private set; }

    public CachedVoiceState(IGatewayClient client, Snowflake guildId, VoiceStateJsonModel model)
        : base(client)
    {
        GuildId = guildId;
        MemberId = model.UserId;

        Update(model);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Update(VoiceStateJsonModel model)
    {
        ChannelId = model.ChannelId;
        SessionId = model.SessionId;
        IsDeafened = model.Deaf;
        IsMuted = model.Mute;
        IsSelfDeafened = model.SelfDeaf;
        IsSelfMuted = model.SelfMute;
        IsStreaming = model.SelfStream.GetValueOrDefault();
        IsTransmittingVideo = model.SelfVideo;
        RequestedToSpeakAt = model.RequestToSpeakTimestamp;
    }
}

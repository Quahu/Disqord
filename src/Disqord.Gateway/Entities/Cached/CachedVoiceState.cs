using System;
using System.ComponentModel;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway
{
    public class CachedVoiceState : CachedEntity, IVoiceState
    {
        public Snowflake GuildId { get; }

        public Snowflake MemberId { get; }

        public Snowflake? ChannelId { get; private set; }

        public string SessionId { get; private set; }

        public bool IsDeafened { get; private set; }

        public bool IsMuted { get; private set; }

        public bool IsSelfDeafened { get; private set; }

        public bool IsSelfMuted { get; private set; }

        public bool IsStreaming { get; private set; }

        public bool IsTransmittingVideo { get; private set; }

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
}

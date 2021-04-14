using System;
using System.ComponentModel;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class CachedVoiceState : CachedSnowflakeEntity, IVoiceState
    {
        public Snowflake GuildId { get; }

        public Snowflake? ChannelId { get; private set; }

        public Snowflake MemberId => Id;

        public string SessionId { get; private set; }

        public bool IsDeafened { get; private set; }

        public bool IsMuted { get; private set; }

        public bool IsSelfDeafened { get; private set; }

        public bool IsSelfMuted { get; private set; }

        public bool IsStreaming { get; private set; }

        public bool IsTransmittingVideo { get; private set; }

        public DateTimeOffset? RequestedToSpeakAt { get; private set; }

        public CachedVoiceState(IGatewayClient client, Snowflake guildId, VoiceStateJsonModel model)
            : base(client, model.UserId)
        {
            GuildId = guildId;

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

            if (model.SelfStream.HasValue)
                IsStreaming = model.SelfStream.Value;

            IsTransmittingVideo = model.SelfVideo;
            RequestedToSpeakAt = model.RequestToSpeakTimestamp;
        }
    }
}

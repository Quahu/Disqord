using System;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class TransientVoiceState : TransientEntity<VoiceStateJsonModel>, IVoiceState
    {
        public Snowflake GuildId => Model.GuildId.Value;

        public Snowflake? ChannelId => Model.ChannelId;

        public Snowflake MemberId => Model.UserId;

        public string SessionId => Model.SessionId;

        public bool IsDeafened => Model.Deaf;

        public bool IsMuted => Model.Mute;

        public bool IsSelfDeafened => Model.SelfDeaf;

        public bool IsSelfMuted => Model.SelfMute;

        public bool IsStreaming => Model.SelfStream.GetValueOrDefault();

        public bool IsTransmittingVideo => Model.SelfVideo;

        public DateTimeOffset? RequestedToSpeakAt => Model.RequestToSpeakTimestamp;

        public TransientVoiceState(IClient client, VoiceStateJsonModel model)
            : base(client, model)
        { }
    }
}

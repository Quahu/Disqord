using Disqord.Models;

namespace Disqord
{
    public sealed class VoiceState
    {
        public string SessionId { get; private set; }

        public Snowflake ChannelId { get; private set; }

        public bool IsMuted { get; private set; }

        public bool IsDeafened { get; private set; }

        public bool IsSelfMuted { get; private set; }

        public bool IsSelfDeafened { get; private set; }

        public bool IsStreaming { get; private set; }

        public bool IsVideoStreaming { get; private set; }

        public bool IsSuppressed { get; private set; }

        internal VoiceState(VoiceStateModel model)
        {
            Update(model);
        }

        internal void Update(VoiceStateModel model)
        {
            if (model.ChannelId.HasValue)
                ChannelId = model.ChannelId.Value.Value;

            if (model.Mute.HasValue)
                IsMuted = model.Mute.Value;

            if (model.Deaf.HasValue)
                IsDeafened = model.Deaf.Value;

            if (model.SelfMute.HasValue)
                IsSelfMuted = model.SelfMute.Value;

            if (model.SelfDeaf.HasValue)
                IsSelfDeafened = model.SelfDeaf.Value;

            if (model.SelfStream.HasValue)
                IsStreaming = model.SelfStream.Value;

            if (model.SelfVideo.HasValue)
                IsVideoStreaming = model.SelfVideo.Value;

            if (model.Suppress.HasValue)
                IsSuppressed = model.Suppress.Value;

            if (model.SessionId.HasValue)
                SessionId = model.SessionId.Value;
        }

        internal VoiceState Clone()
            => (VoiceState) MemberwiseClone();
    }
}

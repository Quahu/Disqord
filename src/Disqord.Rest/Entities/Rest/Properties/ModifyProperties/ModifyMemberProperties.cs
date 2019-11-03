using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyMemberProperties
    {
        public Optional<string> Nick { internal get; set; }

        public Optional<IEnumerable<Snowflake>> RoleIds { internal get; set; }

        public Optional<bool> Mute { internal get; set; }

        public Optional<bool> Deaf { internal get; set; }

        public Optional<Snowflake?> VoiceChannelId { internal get; set; }

        internal ModifyMemberProperties()
        { }

        internal bool HasValues
            => Nick.HasValue || RoleIds.HasValue || Mute.HasValue || Deaf.HasValue || VoiceChannelId.HasValue;
    }
}

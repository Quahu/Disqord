using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public sealed class ModifyMemberActionProperties
    {
        public Optional<string> Nick { internal get; set; }

        public Optional<IEnumerable<Snowflake>> RoleIds { internal get; set; }

        public Optional<bool> Mute { internal get; set; }

        public Optional<bool> Deaf { internal get; set; }

        public Optional<Snowflake?> VoiceChannelId { internal get; set; }

        public Optional<DateTimeOffset?> TimedOutUntil { internal get; set; }

        internal bool HasValues
            => Nick.HasValue || RoleIds.HasValue || Mute.HasValue || Deaf.HasValue || VoiceChannelId.HasValue || TimedOutUntil.HasValue;
    }
}

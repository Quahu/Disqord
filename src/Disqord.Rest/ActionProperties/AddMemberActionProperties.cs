using System.Collections.Generic;
using Qommon;

namespace Disqord.Rest
{
    public class AddMemberActionProperties
    {
        /// <summary>
        ///     Gets or sets the nick the member should have upon join.
        /// </summary>
        public Optional<string> Nick { internal get; set; }

        /// <summary>
        ///     Gets or sets the IDs of roles the member should have upon join.
        /// </summary>
        public Optional<IEnumerable<Snowflake>> RoleIds { internal get; set; }

        /// <summary>
        ///     Gets or sets whether the member should be muted in voice channels upon join.
        /// </summary>
        public Optional<bool> IsMuted { internal get; set; }

        /// <summary>
        ///     Gets or sets whether the member should be deafened in voice channels upon join.
        /// </summary>
        public Optional<bool> IsDeafened { internal get; set; }
    }
}

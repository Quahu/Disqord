using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyGuildEmojiProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<IReadOnlyList<ulong>> RoleIds { internal get; set; }

        internal ModifyGuildEmojiProperties()
        { }

        internal bool HasValues
            => Name.HasValue || RoleIds.HasValue;
    }
}

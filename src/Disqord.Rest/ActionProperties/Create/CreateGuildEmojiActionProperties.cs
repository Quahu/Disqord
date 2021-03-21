using System.Collections.Generic;

namespace Disqord
{
    public sealed class CreateGuildEmojiActionProperties
    {
        public Optional<IEnumerable<Snowflake>> RoleIds { internal get; set; }
    }
}

using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public sealed class CreateGuildEmojiActionProperties
    {
        public Optional<IEnumerable<Snowflake>> RoleIds { internal get; set; }
    }
}

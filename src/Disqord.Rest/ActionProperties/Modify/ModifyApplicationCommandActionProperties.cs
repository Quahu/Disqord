using System.Collections.Generic;

namespace Disqord.Rest
{
    public sealed class ModifyApplicationCommandActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<IEnumerable<LocalApplicationCommandOption>> Options { internal get; set; }

        public Optional<bool> EnabledByDefault { internal get; set; }
    }
}

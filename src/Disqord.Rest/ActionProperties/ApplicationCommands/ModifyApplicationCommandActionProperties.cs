using System.Collections.Generic;
using Qommon;

namespace Disqord.Rest
{
    public sealed class ModifyApplicationCommandActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<bool> IsEnabledByDefault { internal get; set; }

        public Optional<IEnumerable<LocalSlashCommandOption>> Options { internal get; set; }
    }
}

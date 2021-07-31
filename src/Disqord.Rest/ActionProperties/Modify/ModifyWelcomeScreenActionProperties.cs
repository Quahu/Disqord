using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyWelcomeScreenActionProperties
    {
        public Optional<bool> Enabled { internal get; set; }

        public Optional<IEnumerable<LocalGuildWelcomeScreenChannel>> Channels { internal get; set; }

        public Optional<string> Description { internal get; set; }
    }
}

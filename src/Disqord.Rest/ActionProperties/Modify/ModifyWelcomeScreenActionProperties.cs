using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public sealed class ModifyWelcomeScreenActionProperties
    {
        public Optional<bool> IsEnabled { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<IEnumerable<LocalGuildWelcomeScreenChannel>> Channels { internal get; set; }
    }
}

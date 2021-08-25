using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed class ModifyApplicationCommandActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> Description { internal get; set; }

        public Optional<LocalApplicationCommandOption> Options { internal get; set; }

        public Optional<bool> EnabledByDefault { internal get; set; }
    }
}

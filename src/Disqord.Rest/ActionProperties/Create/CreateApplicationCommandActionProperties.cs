using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed class CreateApplicationCommandActionProperties
    {
        public Optional<IEnumerable<LocalApplicationCommandOption>> Options { internal get; set; }

        public Optional<bool> IsEnabledByDefault { internal get; set; }

        public Optional<ApplicationCommandType> Type { internal get; set; }
    }
}

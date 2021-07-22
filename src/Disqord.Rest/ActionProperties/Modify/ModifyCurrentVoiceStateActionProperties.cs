using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public sealed class ModifyCurrentVoiceStateActionProperties
    {
        public Optional<bool> Suppress { internal get; set; }

        public Optional<DateTimeOffset> RequestToSpeakTimestamp { internal get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed class ModifyCurrentMemberVoiceStateActionProperties : ModifyMemberVoiceStateActionProperties
    {
        public Optional<DateTimeOffset?> RequestedToSpeakAt { internal get; set; }

        internal ModifyCurrentMemberVoiceStateActionProperties() { }
    }
}

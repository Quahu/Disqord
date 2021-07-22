using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public class ModifyMemberVoiceStateActionProperties
    {
        public Optional<bool> IsSuppressed { internal get; set; }

        internal ModifyMemberVoiceStateActionProperties() 
        { }
    }
}

using System;
using Qommon;

namespace Disqord.Rest
{
    public sealed class ModifyCurrentMemberVoiceStateActionProperties : ModifyMemberVoiceStateActionProperties
    {
        public Optional<DateTimeOffset?> RequestedToSpeakAt { internal get; set; }

        internal ModifyCurrentMemberVoiceStateActionProperties()
        { }
    }
}

using System;

namespace Disqord.Gateway
{
    public class VoiceStateUpdatedEventArgs : EventArgs
    {
        public IVoiceState VoiceState { get; }

        public VoiceStateUpdatedEventArgs(IVoiceState voiceState)
        {
            VoiceState = voiceState;
        }
    }
}

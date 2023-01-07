using System;

namespace Disqord.Voice;

[Flags]
public enum SpeakingFlags
{
    None = 0,

    Microphone = 1 << 0,

    Soundshare = 1 << 1,

    Priority = 1 << 2
}

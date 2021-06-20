using System;

namespace Disqord
{
    [Flags]
    public enum InteractionResponseFlag
    {
        None = 0,

        Ephemeral = 64
    }
}

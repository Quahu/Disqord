using System;

namespace Disqord
{
    public enum TokenType : byte
    {
        Bearer,

        Bot,

        [Obsolete("User tokens and logins are not supported.")]
        User
    }
}

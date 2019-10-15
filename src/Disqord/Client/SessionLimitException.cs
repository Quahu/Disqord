using System;

namespace Disqord
{
    public sealed class SessionLimitException : Exception
    {
        public TimeSpan ResetsAfter { get; }

        public SessionLimitException(TimeSpan resetsAfter)
            => ResetsAfter = resetsAfter;
    }
}
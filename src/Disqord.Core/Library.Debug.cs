using System;
using System.ComponentModel;

namespace Disqord
{
    public static partial class Library
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static class Debug
        {
            public static bool DumpRateLimits;

            public static bool DumpJson;

            public static IDisposable DumpingRateLimits()
                => new DumpingBlock(() => DumpRateLimits = !DumpRateLimits);

            public static IDisposable DumpingJson()
                => new DumpingBlock(() => DumpJson = !DumpJson);

            private sealed class DumpingBlock : IDisposable
            {
                private readonly Action _action;

                public DumpingBlock(Action action)
                {
                    _action = action;
                    action();
                }

                public void Dispose()
                    => _action();
            }
        }
    }
}
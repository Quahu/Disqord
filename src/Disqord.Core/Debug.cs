#if DEBUG
using System;
using System.ComponentModel;

namespace Disqord
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class Debug
    {
        public static bool DumpJson;

        public static IDisposable DumpingJson()
            => new DumpingJsonBlock();

        private sealed class DumpingJsonBlock : IDisposable
        {
            public DumpingJsonBlock()
            {
                DumpJson = true;
            }

            public void Dispose()
                => DumpJson = false;
        }
    }
}
#endif

using System;
using System.ComponentModel;
using System.IO;

namespace Disqord
{
    public static partial class Library
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static class Debug
        {
            public static bool DumpRateLimits;

            public static bool DumpJson;

            public static bool TimedWebSocketConnect;

            public static TextWriter DumpWriter
            {
                get => _dumpWriter;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));

                    _dumpWriter = value;
                }
            }
            private static TextWriter _dumpWriter = Console.Out;

            public static IDisposable DumpingRateLimits(TextWriter customWriter = null)
                => new DumpingBlock(() => DumpRateLimits = !DumpRateLimits, customWriter);

            public static IDisposable DumpingJson(TextWriter customWriter = null)
                => new DumpingBlock(() => DumpJson = !DumpJson, customWriter);

            private sealed class DumpingBlock : IDisposable
            {
                private readonly Action _action;
                private readonly TextWriter _previousWriter;

                public DumpingBlock(Action action, TextWriter writer = null)
                {
                    _action = action;
                    if (writer != null)
                    {
                        _previousWriter = DumpWriter;
                        DumpWriter = writer;
                    }

                    action();
                }

                public void Dispose()
                {
                    if (_previousWriter != null)
                        DumpWriter = _previousWriter;

                    _action();
                }
            }
        }
    }
}
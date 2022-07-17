using System;
using System.ComponentModel;
using System.IO;
using Qommon;

namespace Disqord;

public static partial class Library
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Debug
    {
        public static bool DumpJson;

        public static TextWriter DumpWriter
        {
            get => _dumpWriter;
            set
            {
                Guard.IsNotNull(value);

                _dumpWriter = value;
            }
        }
        private static TextWriter _dumpWriter = Console.Out;

        public static IDisposable DumpingJson(TextWriter? customWriter = null)
            => new DumpingBlock(() => DumpJson = !DumpJson, customWriter);

        private sealed class DumpingBlock : IDisposable
        {
            private readonly Action _action;
            private readonly TextWriter? _previousWriter;

            public DumpingBlock(Action action, TextWriter? writer = null)
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

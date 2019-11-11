using System;
using System.ComponentModel;

namespace Disqord
{
    /// <summary>
    ///     Provides utilities related to the Disqord library.
    /// </summary>
    public static class Library
    {
        /// <summary>
        ///     Disqord build's version.
        /// </summary>
        public static readonly Version Version = typeof(Library).Assembly.GetName().Version;

        // TODO: set at compile-time
        ///// <summary>
        /////     Disqord build's date.
        ///// </summary>
        //public static readonly DateTimeOffset BuiltAt;

        /// <summary>
        ///     Disqord's repository url.
        /// </summary>
        public static readonly string RepositoryUrl = "https://github.com/Quahu/Disqord";

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
}
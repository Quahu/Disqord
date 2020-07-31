using System;
using Disqord.Logging.Default;

namespace Disqord.Logging
{
    public interface ILogger : IDisposable
    {
        /// <summary>
        ///     An event which should trigger when the implementing logger logs something.
        /// </summary>
        event EventHandler<LogEventArgs> Logged;

        void Log(object sender, LogEventArgs e);

        internal static ILogger CreateDefault()
            => new DefaultLogger();
    }
}

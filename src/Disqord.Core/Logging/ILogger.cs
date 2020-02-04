using System;

namespace Disqord.Logging
{
    public interface ILogger
    {
        event EventHandler<MessageLoggedEventArgs> MessageLogged;

        void Log(object sender, MessageLoggedEventArgs e);
    }
}

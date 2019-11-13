using System;

namespace Disqord.Logging
{
    // TODO: disposable?
    public interface ILogger
    {
        event EventHandler<MessageLoggedEventArgs> MessageLogged;

        void Log(object sender, MessageLoggedEventArgs e);
    }
}

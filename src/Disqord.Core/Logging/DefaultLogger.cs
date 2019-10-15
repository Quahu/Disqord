using System;

namespace Disqord.Logging
{
    public sealed class DefaultLogger : ILogger
    {
        private readonly object _lock = new object();

        public event EventHandler<MessageLoggedEventArgs> MessageLogged;

        public void Log(object sender, MessageLoggedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            if (e == null)
                throw new ArgumentNullException(nameof(e));

            Delegate[] list;
            lock (_lock)
            {
                list = MessageLogged?.GetInvocationList();
            }

            if (list == null)
                return;

            for (var i = 0; i < list.Length; i++)
            {
                var handler = list[i] as EventHandler<MessageLoggedEventArgs>;
                try
                {
                    handler(sender, e);
                }
                catch { }
            }
        }
    }
}
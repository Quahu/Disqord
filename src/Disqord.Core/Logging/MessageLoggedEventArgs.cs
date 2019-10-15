using System;

namespace Disqord.Logging
{
    public sealed class MessageLoggedEventArgs : EventArgs
    {
        public string Source { get; }

        public LogMessageSeverity Severity { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public DateTime TimeStamp { get; } = DateTime.Now;

        public MessageLoggedEventArgs(string source, LogMessageSeverity severity, string message, Exception exception = null)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Severity = severity;
            Message = message;
            Exception = exception;
        }

        public override string ToString()
            => $"[{TimeStamp}] [{Source}] [{Severity}] {Message}{(Exception != null ? $"\n{Exception}" : "")}";
    }
}
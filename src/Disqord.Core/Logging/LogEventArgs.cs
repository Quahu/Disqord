using System;

namespace Disqord.Logging
{
    public sealed class LogEventArgs : EventArgs
    {
        public string Source { get; }

        public LogSeverity Severity { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public DateTime TimeStamp { get; } = DateTime.Now;

        public LogEventArgs(string source, LogSeverity severity, string message, Exception exception = null)
        {
            Source = source;
            Severity = severity;
            Message = message;
            Exception = exception;
        }

        public override string ToString()
            => $"[{TimeStamp:HH:mm:ss}] [{Source}] [{Severity}] {Message}{(Exception != null ? $"\n{Exception}" : "")}";
    }
}
using System;
using Microsoft.Extensions.Logging;

namespace Disqord.Test
{
    public class TestLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public TestLogger(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(typeof(T).Name);
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}

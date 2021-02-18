using Microsoft.Extensions.Logging;

namespace Disqord.Logging
{
    /// <summary>
    ///     Represents a logging entity.
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        ///     Gets the logger of this entity.
        /// </summary>
        ILogger Logger { get; }
    }
}

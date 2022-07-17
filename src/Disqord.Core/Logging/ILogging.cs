using Microsoft.Extensions.Logging;

namespace Disqord.Logging;

/// <summary>
///     Represents a logging construct.
/// </summary>
public interface ILogging
{
    /// <summary>
    ///     Gets the logger of this construct.
    /// </summary>
    ILogger Logger { get; }
}
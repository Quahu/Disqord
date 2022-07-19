using System.Collections.Generic;

namespace Disqord.Http;

/// <summary>
///     Represents an HTTP construct with headers for sent or received HTTP data.
/// </summary>
public interface IHeaders
{
    /// <summary>
    ///     Gets the headers associated with this HTTP construct.
    /// </summary>
    IDictionary<string, string> Headers { get; }
}

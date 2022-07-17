using System;
using System.Collections.Generic;

namespace Disqord.Http;

/// <summary>
///     Represents HTTP headers for sent or received HTTP data.
///     This base class implements <see cref="IHeaders"/> and exists to reduce code duplication.
/// </summary>
public abstract class HeadersBase : IHeaders
{
    /// <inheritdoc/>
    public IDictionary<string, string> Headers { get; }

    /// <summary>
    ///     Instantiates a new <see cref="HeadersBase"/> with the default case-insensitive comparer based <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    protected HeadersBase()
    {
        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Instantiates a new <see cref="HeadersBase"/> with the specified dictionary.
    /// </summary>
    /// <param name="headers"></param>
    protected HeadersBase(IDictionary<string, string> headers)
    {
        Headers = headers;
    }
}
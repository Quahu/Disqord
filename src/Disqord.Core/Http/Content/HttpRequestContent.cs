using System;

namespace Disqord.Http;

/// <summary>
///     Represents HTTP content.
/// </summary>
public abstract class HttpRequestContent : HeadersBase, IDisposable
{
    /// <summary>
    ///     Disposes the resources held by this content.
    /// </summary>
    public virtual void Dispose()
    { }
}
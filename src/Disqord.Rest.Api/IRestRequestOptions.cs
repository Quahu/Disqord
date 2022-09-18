using System.Collections.Generic;
using Disqord.Rest.Api;

namespace Disqord.Rest;

/// <summary>
///     Represents a set of options for a REST request.
/// </summary>
public interface IRestRequestOptions
{
    /// <summary>
    ///     Gets or sets the audit log reason for the request.
    /// </summary>
    /// <remarks>
    ///     This is a shorthand for setting the <see cref="RestApiHeaderNames.AuditLogReason"/> header in <see cref="Headers"/>.
    /// </remarks>
    string? Reason
    {
        get
        {
            if (Headers.TryGetValue(RestApiHeaderNames.AuditLogReason, out var value))
                return value;

            return null;
        }
        set
        {
            if (value == null)
            {
                Headers.Remove(RestApiHeaderNames.AuditLogReason);
            }
            else
            {
                Headers[RestApiHeaderNames.AuditLogReason] = value;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the headers for the request.
    /// </summary>
    IDictionary<string, string> Headers { get; set; }
}

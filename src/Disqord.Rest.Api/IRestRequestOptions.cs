using System.Collections.Generic;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents a set of options for a REST request.
    /// </summary>
    public interface IRestRequestOptions
    {
        /// <summary>
        ///     Gets or sets the audit log reason for the request.
        ///     This is a shortcut for setting the <c>X-Audit-Log-Reason</c> header in <see cref="Headers"/>.
        /// </summary>
        string Reason
        {
            get
            {
                if (Headers != null && Headers.TryGetValue("X-Audit-Log-Reason", out var value))
                    return value;

                return null;
            }
            set => Headers["X-Audit-Log-Reason"] = value;
        }

        /// <summary>
        ///     Gets or sets the headers for the request.
        /// </summary>
        IDictionary<string, string> Headers { get; set; }
    }
}

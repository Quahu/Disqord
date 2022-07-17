using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents the request content of a REST request
///     with support for attachments.
/// </summary>
public interface IAttachmentRestRequestContent
{
    /// <summary>
    ///     Sets the attachments of this content.
    /// </summary>
    IList<PartialAttachmentJsonModel> Attachments { set; }
}

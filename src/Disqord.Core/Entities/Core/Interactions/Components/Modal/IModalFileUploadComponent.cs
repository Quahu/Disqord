using System.Collections.Generic;

namespace Disqord;

/// <inheritdoc cref="IModalComponent"/>
public interface IModalFileUploadComponent : IModalComponent, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the IDs of the attachments submitted in the modal.
    /// </summary>
    IReadOnlyList<Snowflake> AttachmentIds { get; }
}

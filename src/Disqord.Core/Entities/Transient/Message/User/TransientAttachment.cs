using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientAttachment : TransientEntity<AttachmentJsonModel>, IAttachment
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id.Value;

    /// <inheritdoc/>
    public string FileName => Model.FileName;

    /// <inheritdoc/>
    public string? Description => Model.Description.GetValueOrDefault();

    /// <inheritdoc/>
    public string? ContentType => Model.ContentType.GetValueOrDefault();

    /// <inheritdoc/>
    public int FileSize => Model.Size;

    /// <inheritdoc/>
    public string Url => Model.Url;

    /// <inheritdoc/>
    public string ProxyUrl => Model.ProxyUrl;

    /// <inheritdoc/>
    public int? Width => Model.Width.GetValueOrNullable();

    /// <inheritdoc/>
    public int? Height => Model.Height.GetValueOrNullable();

    /// <inheritdoc/>
    public bool IsEphemeral => Model.Ephemeral.GetValueOrDefault();

    public TransientAttachment(AttachmentJsonModel model)
        : base(model)
    { }

    public override string ToString()
    {
        return $"Attachment '{FileName}' ({Id})";
    }
}

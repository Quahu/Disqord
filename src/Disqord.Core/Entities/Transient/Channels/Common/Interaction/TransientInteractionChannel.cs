using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IInteractionChannel"/>
public class TransientInteractionChannel : TransientChannel, IInteractionChannel
{
    /// <inheritdoc/>
    public Permissions AuthorPermissions => Model.Permissions.GetValueOrDefault();

    /// <inheritdoc/>
    public Snowflake? ParentId => Model.ParentId.GetValueOrDefault();

    /// <inheritdoc/>
    public IThreadMetadata? ThreadMetadata
    {
        get
        {
            if (!Model.ThreadMetadata.HasValue)
                return null;

            return _threadMetadata ??= new TransientThreadMetadata(Model.ThreadMetadata.Value);
        }
    }
    private IThreadMetadata? _threadMetadata;

    public TransientInteractionChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}

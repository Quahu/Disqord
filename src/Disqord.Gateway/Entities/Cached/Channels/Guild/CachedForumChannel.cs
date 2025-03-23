using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

/// <inheritdoc cref="IForumChannel"/>
public class CachedForumChannel : CachedMediaChannel, IForumChannel
{
    /// <inheritdoc/>
    public ForumLayout DefaultLayout { get; private set; }

    public CachedForumChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);
        DefaultLayout = model.DefaultForumLayout.GetValueOrDefault();
    }
}

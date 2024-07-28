using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IForumChannel"/>
public class TransientForumChannel : TransientMediaChannel, IForumChannel
{
    /// <inheritdoc/>
    public ForumLayout DefaultLayout => Model.DefaultForumLayout.GetValueOrDefault();

    public TransientForumChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}

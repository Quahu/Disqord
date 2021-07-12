using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IStoreChannel"/>
    public class TransientStoreChannel : TransientCategorizableGuildChannel, IStoreChannel
    {
        /// <inheritdoc/>
        public bool IsNsfw => Model.Nsfw.Value;

        public TransientStoreChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

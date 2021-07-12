using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IVocalGuildChannel"/>
    public abstract class TransientVocalGuildChannel : TransientCategorizableGuildChannel, IVocalGuildChannel
    {
        /// <inheritdoc/>
        public int Bitrate => Model.Bitrate.Value;

        /// <inheritdoc/>
        public string Region => Model.RtcRegion.Value;

        protected TransientVocalGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

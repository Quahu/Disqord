using Disqord.Models;

namespace Disqord
{
    public class TransientInviteChannel : TransientEntity<ChannelJsonModel>, IInviteChannel
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name.Value;

        /// <inheritdoc/>
        public ChannelType Type => Model.Type;

        public TransientInviteChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
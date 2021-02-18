using System;
using Disqord.Models;

namespace Disqord
{
    public abstract class TransientPrivateChannel : TransientChannel, IPrivateChannel
    {
        public Snowflake? LastMessageId => Model.LastMessageId.GetValueOrDefault();

        public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.GetValueOrDefault();

        public TransientPrivateChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

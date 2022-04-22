using System;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientPrivateChannel : TransientChannel, IPrivateChannel
    {
        public Snowflake? LastMessageId => Model.LastMessageId.GetValueOrDefault();

        public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.GetValueOrDefault();

        protected TransientPrivateChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

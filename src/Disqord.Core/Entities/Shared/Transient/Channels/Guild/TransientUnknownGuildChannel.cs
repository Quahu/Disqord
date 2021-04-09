using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientUnknownGuildChannel : TransientGuildChannel, IUnknownGuildChannel
    {
        public Snowflake? CategoryId { get; }

        public ChannelType Type { get; }

        public TransientUnknownGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => Name;
    }
}

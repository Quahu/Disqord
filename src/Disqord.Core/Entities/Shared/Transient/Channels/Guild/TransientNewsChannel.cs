using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientNewsChannel : TransientTextChannel, INewsChannel
    {
        public TransientNewsChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

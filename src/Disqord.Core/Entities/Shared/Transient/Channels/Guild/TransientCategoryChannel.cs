using Disqord.Models;

namespace Disqord
{
    public class TransientCategoryChannel : TransientGuildChannel, ICategoryChannel
    {
        public TransientCategoryChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}

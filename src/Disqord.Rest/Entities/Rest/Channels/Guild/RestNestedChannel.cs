using Disqord.Models;

namespace Disqord.Rest
{
    public abstract class RestNestedChannel : RestGuildChannel, INestedChannel
    {
        public Snowflake? CategoryId { get; private set; }

        internal RestNestedChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

            base.Update(model);
        }
    }
}
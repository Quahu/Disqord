using System;
using Disqord.Models;

namespace Disqord
{
    public abstract class TransientChannel : TransientEntity<ChannelJsonModel>, IChannel
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public virtual string Name => Model.Name.Value;

        public TransientChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => Name;

        public static TransientChannel Create(IClient client, ChannelJsonModel model)
        {
            switch (model.Type)
            {
                case ChannelType.Direct:
                    return new TransientDirectChannel(client, model);

                case ChannelType.Group:
                    break;

                case ChannelType.Text:
                case ChannelType.Voice:
                case ChannelType.Category:
                case ChannelType.News:
                case ChannelType.Store:
                case ChannelType.Thread:
                    return TransientGuildChannel.Create(client, model);
            }

            return null!/*TransientUnknownChannel(client, model)*/;
        }
    }
}

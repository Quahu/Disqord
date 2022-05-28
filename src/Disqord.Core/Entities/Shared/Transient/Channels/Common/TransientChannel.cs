using Disqord.Models;

namespace Disqord
{
    public abstract class TransientChannel : TransientClientEntity<ChannelJsonModel>, IChannel
    {
        public Snowflake Id => Model.Id;

        public virtual string Name => Model.Name.Value;

        public ChannelType Type => Model.Type;

        protected TransientChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => Name;

        public static IChannel Create(IClient client, ChannelJsonModel model)
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
                case ChannelType.NewsThread:
                case ChannelType.PublicThread:
                case ChannelType.PrivateThread:
                case ChannelType.Stage:
                case ChannelType.Directory:
                    return TransientGuildChannel.Create(client, model);
            }

            return null /*TransientUnknownChannel(client, model)*/;
        }
    }
}

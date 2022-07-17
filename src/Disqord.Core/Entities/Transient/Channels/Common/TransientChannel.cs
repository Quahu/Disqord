using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IChannel"/>
public abstract class TransientChannel : TransientClientEntity<ChannelJsonModel>, IChannel
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public virtual string Name => Model.Name.Value;

    /// <inheritdoc/>
    public ChannelType Type => Model.Type;

    protected TransientChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    public override string ToString()
    {
        return Name;
    }

    public static IChannel? Create(IClient client, ChannelJsonModel model)
    {
        switch (model.Type)
        {
            case ChannelType.Direct:
                return new TransientDirectChannel(client, model);

            case ChannelType.Group:
                return new TransientGroupChannel(client, model);

            case ChannelType.Text:
            case ChannelType.Voice:
            case ChannelType.Category:
            case ChannelType.News:
            case ChannelType.NewsThread:
            case ChannelType.PublicThread:
            case ChannelType.PrivateThread:
            case ChannelType.Stage:
            case ChannelType.Directory:
            case ChannelType.Forum:
                return TransientGuildChannel.Create(client, model);
        }

        return null;
    }
}

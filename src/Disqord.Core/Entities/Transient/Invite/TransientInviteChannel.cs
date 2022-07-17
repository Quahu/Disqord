using Disqord.Models;

namespace Disqord;

public class TransientInviteChannel : TransientClientEntity<ChannelJsonModel>, IInviteChannel
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name.Value;

    /// <inheritdoc/>
    public ChannelType Type => Model.Type;

    public TransientInviteChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}

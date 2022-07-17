using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IOverwrite"/>
public class TransientOverwrite : TransientClientEntity<OverwriteJsonModel>, IOverwrite
{
    /// <inheritdoc/>
    public Snowflake ChannelId { get; }

    /// <inheritdoc/>
    public Snowflake TargetId => Model.Id;

    /// <inheritdoc/>
    public OverwriteTargetType TargetType => Model.Type;

    /// <inheritdoc/>
    public OverwritePermissions Permissions => new(Model.Allow, Model.Deny);

    public TransientOverwrite(IClient client, Snowflake channelId, OverwriteJsonModel model)
        : base(client, model)
    {
        ChannelId = channelId;
    }
}
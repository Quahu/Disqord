using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IStageChannel"/>
public class TransientStageChannel : TransientCategorizableGuildChannel, IStageChannel
{
    /// <inheritdoc/>
    public int Bitrate => Model.Bitrate.Value;

    /// <inheritdoc/>
    public string Region => Model.RtcRegion.Value;

    public TransientStageChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}

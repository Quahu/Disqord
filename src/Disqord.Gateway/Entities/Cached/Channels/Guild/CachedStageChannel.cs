using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IStageChannel"/>
public class CachedStageChannel : CachedCategorizableGuildChannel, IStageChannel
{
    /// <inheritdoc/>
    public int Bitrate { get; private set; }

    /// <inheritdoc/>
    public string? Region { get; private set; }

    public CachedStageChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (model.Bitrate.HasValue)
            Bitrate = model.Bitrate.Value;

        if (model.RtcRegion.HasValue)
            Region = model.RtcRegion.Value;
    }
}

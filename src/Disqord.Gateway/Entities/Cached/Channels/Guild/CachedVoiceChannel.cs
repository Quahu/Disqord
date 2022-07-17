using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

/// <inheritdoc cref="IVoiceChannel"/>
public class CachedVoiceChannel : CachedMessageGuildChannel, IVoiceChannel
{
    /// <inheritdoc/>
    public int Bitrate { get; private set; }

    /// <inheritdoc/>
    public string? Region { get; private set; }

    /// <inheritdoc/>
    public int MemberLimit { get; private set; }

    /// <inheritdoc/>
    public VideoQualityMode VideoQualityMode { get; private set; }

    /// <inheritdoc/>
    public bool IsAgeRestricted { get; private set; }

    public CachedVoiceChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    {
        VideoQualityMode = model.VideoQualityMode.GetValueOrDefault(VideoQualityMode.Automatic);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (model.Bitrate.HasValue)
            Bitrate = model.Bitrate.Value;

        if (model.RtcRegion.HasValue)
            Region = model.RtcRegion.Value;

        if (model.UserLimit.HasValue)
            MemberLimit = model.UserLimit.Value;

        if (model.VideoQualityMode.HasValue)
            VideoQualityMode = model.VideoQualityMode.Value;

        if (model.Nsfw.HasValue)
            IsAgeRestricted = model.Nsfw.Value;
    }
}

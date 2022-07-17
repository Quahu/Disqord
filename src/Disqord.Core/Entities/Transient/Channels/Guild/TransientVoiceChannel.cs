using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IVoiceChannel"/>
public class TransientVoiceChannel : TransientMessageGuildChannel, IVoiceChannel
{
    /// <inheritdoc/>
    public int Bitrate => Model.Bitrate.Value;

    /// <inheritdoc/>
    public string Region => Model.RtcRegion.Value;

    /// <inheritdoc/>
    public int MemberLimit => Model.UserLimit.Value;

    /// <inheritdoc/>
    public VideoQualityMode VideoQualityMode => Model.VideoQualityMode.GetValueOrDefault(VideoQualityMode.Automatic);

    /// <inheritdoc/>
    public bool IsAgeRestricted => Model.Nsfw.Value;

    public TransientVoiceChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}

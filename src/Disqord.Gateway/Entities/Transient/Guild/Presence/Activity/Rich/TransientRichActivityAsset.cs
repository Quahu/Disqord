using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientRichActivityAsset : TransientEntity<ActivityAssetsJsonModel>, IRichActivityAsset
{
    /// <inheritdoc/>
    public Snowflake? ApplicationId { get; }

    /// <inheritdoc/>
    public string? Id => (_isLarge ? Model.LargeImage : Model.SmallImage).GetValueOrDefault();

    /// <inheritdoc/>
    public string? Text => (_isLarge ? Model.LargeText : Model.SmallText).GetValueOrDefault();

    private readonly bool _isLarge;

    public TransientRichActivityAsset(Snowflake? applicationId, bool isLarge, ActivityAssetsJsonModel model)
        : base(model)
    {
        ApplicationId = applicationId;
        _isLarge = isLarge;
    }
}

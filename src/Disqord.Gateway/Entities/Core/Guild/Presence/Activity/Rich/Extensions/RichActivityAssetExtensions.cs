namespace Disqord.Gateway;

public static class RichActivityAssetExtensions
{
    public static string? GetUrl(this IRichActivityAsset asset)
    {
        var applicationId = asset.ApplicationId;
        var assetId = asset.Id;
        return applicationId != null && assetId != null
            ? Discord.Cdn.GetApplicationAssetUrl(applicationId.Value, assetId)
            : null;
    }
}

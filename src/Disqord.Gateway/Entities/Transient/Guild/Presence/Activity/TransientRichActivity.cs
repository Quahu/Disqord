using System;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientRichActivity : TransientActivity, IRichActivity
{
    /// <inheritdoc/>
    public string? State => Model.Name;

    /// <inheritdoc/>
    public string? Details => Model.Details.GetValueOrDefault();

    /// <inheritdoc/>
    public DateTimeOffset? StartedAt
    {
        get
        {
            if (!Model.Timestamps.HasValue || !Model.Timestamps.Value.Start.HasValue || !long.TryParse(Model.Timestamps.Value.Start.Value, out var start))
                return null;

            try
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(start);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <inheritdoc/>
    public DateTimeOffset? EndsAt
    {
        get
        {
            if (!Model.Timestamps.HasValue || !Model.Timestamps.Value.End.HasValue || !long.TryParse(Model.Timestamps.Value.End.Value, out var end))
                return null;

            try
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(end);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <inheritdoc/>
    public IRichActivityAsset? LargeAsset
    {
        get
        {
            if (!Model.Assets.HasValue)
                return null;

            var assets = Model.Assets.Value;
            if (!assets.LargeImage.HasValue && !assets.LargeText.HasValue)
                return null;

            return _largeAsset ??= new TransientRichActivityAsset(ApplicationId, true, assets);
        }
    }
    private IRichActivityAsset? _largeAsset;

    /// <inheritdoc/>
    public IRichActivityAsset? SmallAsset
    {
        get
        {
            if (!Model.Assets.HasValue)
                return null;

            var assets = Model.Assets.Value;
            if (!assets.SmallImage.HasValue && !assets.SmallText.HasValue)
                return null;

            return _smallAsset ??= new TransientRichActivityAsset(ApplicationId, true, assets);
        }
    }
    private IRichActivityAsset? _smallAsset;

    /// <inheritdoc/>
    public IRichActivityParty? Party
    {
        get
        {
            if (!Model.Party.HasValue)
                return null;

            return _party ??= new TransientRichActivityParty(Model.Party.Value);
        }
    }
    private IRichActivityParty? _party;

    /// <inheritdoc/>
    public string? MatchSecret => Model.Secrets.GetValueOrDefault()?.Match.GetValueOrDefault();

    /// <inheritdoc/>
    public string? JoinSecret => Model.Secrets.GetValueOrDefault()?.Join.GetValueOrDefault();

    /// <inheritdoc/>
    public string? SpectateSecret => Model.Secrets.GetValueOrDefault()?.Spectate.GetValueOrDefault();

    /// <inheritdoc/>
    public bool IsInstanced => Model.Instance.GetValueOrDefault();

    /// <inheritdoc/>
    public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

    /// <inheritdoc/>
    public string? SyncId => Model.SyncId.GetValueOrDefault();

    /// <inheritdoc/>
    public string? SessionId => Model.SessionId.GetValueOrDefault();

    /// <inheritdoc/>
    public ActivityFlags? Flags => Model.Flags.GetValueOrNullable();

    public TransientRichActivity(IClient client, ActivityJsonModel model)
        : base(client, model)
    { }

    public override string ToString()
    {
        return State ?? GetType().ToString();
    }
}

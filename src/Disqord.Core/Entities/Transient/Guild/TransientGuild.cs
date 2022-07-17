using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;
// If you update any members of this class, make sure to do the same for the gateway equivalent.

public class TransientGuild : TransientClientEntity<GuildJsonModel>, IGuild
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public string? IconHash => Model.Icon;

    /// <inheritdoc/>
    public string? SplashHash => Model.Splash;

    /// <inheritdoc/>
    public string? DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

    /// <inheritdoc/>
    public Snowflake OwnerId => Model.OwnerId;

    /// <inheritdoc/>
    public Snowflake? AfkChannelId => Model.AfkChannelId;

    /// <inheritdoc/>
    public TimeSpan AfkTimeout => TimeSpan.FromSeconds(Model.AfkTimeout);

    /// <inheritdoc/>
    public bool IsWidgetEnabled => Model.WidgetEnabled.GetValueOrDefault();

    /// <inheritdoc/>
    public Snowflake? WidgetChannelId => Model.WidgetChannelId.GetValueOrDefault();

    /// <inheritdoc/>
    public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

    /// <inheritdoc/>
    public GuildNotificationLevel NotificationLevel => Model.DefaultMessageNotifications;

    /// <inheritdoc/>
    public GuildContentFilterLevel ContentFilterLevel => Model.ExplicitContentFilter;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IRole> Roles => _roles ??= Model.Roles.ToReadOnlyDictionary((Client, Id),
        (model, _) => model.Id,
        (model, state) =>
        {
            var (client, guildId) = state;
            return new TransientRole(client, guildId, model) as IRole;
        });

    private IReadOnlyDictionary<Snowflake, IRole>? _roles;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id),
        (model, _) => model.Id!.Value,
        (model, state) =>
        {
            var (client, guildId) = state;
            return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
        });

    private IReadOnlyDictionary<Snowflake, IGuildEmoji>? _emojis;

    /// <inheritdoc/>
    public IReadOnlyList<string> Features => Model.Features;

    /// <inheritdoc/>
    public GuildMfaLevel MfaLevel => Model.MfaLevel;

    /// <inheritdoc/>
    public Snowflake? ApplicationId => Model.ApplicationId;

    /// <inheritdoc/>
    public Snowflake? SystemChannelId => Model.SystemChannelId;

    /// <inheritdoc/>
    public SystemChannelFlags SystemChannelFlags => Model.SystemChannelFlags;

    /// <inheritdoc/>
    public Snowflake? RulesChannelId => Model.RulesChannelId;

    /// <inheritdoc/>
    public int? MaxPresenceCount => Model.MaxPresences.GetValueOrDefault();

    /// <inheritdoc/>
    public int? MaxMemberCount => Model.MaxMembers.GetValueOrNullable();

    /// <inheritdoc/>
    public string? VanityUrlCode => Model.VanityUrlCode;

    /// <inheritdoc/>
    public string? Description => Model.Description;

    /// <inheritdoc/>
    public string? BannerHash => Model.Banner;

    /// <inheritdoc/>
    public GuildBoostTier BoostTier => Model.PremiumTier;

    /// <inheritdoc/>
    public int? BoostingMemberCount => Model.PremiumSubscriptionCount.GetValueOrNullable();

    /// <inheritdoc/>
    public CultureInfo PreferredLocale => Discord.Internal.GetLocale(Model.PreferredLocale);

    /// <inheritdoc/>
    public Snowflake? PublicUpdatesChannelId => Model.PublicUpdatesChannelId;

    /// <inheritdoc/>
    public int? MaxVideoMemberCount => Model.MaxVideoChannelUsers.GetValueOrNullable();

    /// <inheritdoc/>
    public GuildNsfwLevel NsfwLevel => Model.NsfwLevel;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IGuildSticker> Stickers
    {
        get
        {
            if (!Model.Stickers.HasValue)
                return ReadOnlyDictionary<Snowflake, IGuildSticker>.Empty;

            return _stickers ??= Model.Stickers.Value.ToReadOnlyDictionary(Client,
                (model, _) => model.Id,
                (model, client) => new TransientGuildSticker(client, model) as IGuildSticker);
        }
    }
    private IReadOnlyDictionary<Snowflake, IGuildSticker>? _stickers;

    /// <inheritdoc/>
    public bool IsBoostProgressBarEnabled => Model.PremiumProgressBarEnabled;

    public TransientGuild(IClient client, GuildJsonModel model)
        : base(client, model)
    { }
}

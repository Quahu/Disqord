using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

public class CachedGuild : CachedSnowflakeEntity, IGatewayGuild,
    IJsonUpdatable<UnavailableGuildJsonModel>, IJsonUpdatable<GuildEmojisUpdateJsonModel>, IJsonUpdatable<GuildMemberAddJsonModel>, IJsonUpdatable<GuildMemberRemoveJsonModel>
{
    // Interface: INamable
    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name { get; private set; } = null!;

    // Interface: IGuild
    /// <inheritdoc/>
    public string? IconHash { get; private set; }

    /// <inheritdoc/>
    public string? SplashHash { get; private set; }

    /// <inheritdoc/>
    public string? DiscoverySplashHash { get; private set; }

    /// <inheritdoc/>
    public Snowflake OwnerId { get; private set; }

    /// <inheritdoc/>
    public Snowflake? AfkChannelId { get; private set; }

    /// <inheritdoc/>
    public TimeSpan AfkTimeout { get; private set; }

    /// <inheritdoc/>
    public bool IsWidgetEnabled { get; private set; }

    /// <inheritdoc/>
    public Snowflake? WidgetChannelId { get; private set; }

    /// <inheritdoc/>
    public GuildVerificationLevel VerificationLevel { get; private set; }

    /// <inheritdoc/>
    public GuildNotificationLevel NotificationLevel { get; private set; }

    /// <inheritdoc/>
    public GuildContentFilterLevel ContentFilterLevel { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IRole> Roles
    {
        get
        {
            if (Client.CacheProvider.TryGetRoles(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedRole, IRole>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IRole>.Empty;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; private set; } = null!;

    /// <inheritdoc/>
    public IReadOnlyList<string> Features { get; private set; } = null!;

    /// <inheritdoc/>
    public GuildMfaLevel MfaLevel { get; private set; }

    /// <inheritdoc/>
    public Snowflake? ApplicationId { get; }

    /// <inheritdoc/>
    public Snowflake? SystemChannelId { get; private set; }

    /// <inheritdoc/>
    public SystemChannelFlags SystemChannelFlags { get; private set; }

    /// <inheritdoc/>
    public Snowflake? RulesChannelId { get; private set; }

    /// <inheritdoc/>
    public int? MaxPresenceCount { get; private set; }

    /// <inheritdoc/>
    public int? MaxMemberCount { get; private set; }

    /// <inheritdoc/>
    public string? VanityUrlCode { get; private set; }

    /// <inheritdoc/>
    public string? Description { get; private set; }

    /// <inheritdoc/>
    public string? BannerHash { get; private set; }

    /// <inheritdoc/>
    public GuildBoostTier BoostTier { get; private set; }

    /// <inheritdoc/>
    public int? BoostingMemberCount { get; private set; }

    /// <inheritdoc/>
    public CultureInfo PreferredLocale { get; private set; } = null!;

    /// <inheritdoc/>
    public Snowflake? PublicUpdatesChannelId { get; private set; }

    /// <inheritdoc/>
    public int? MaxVideoMemberCount { get; private set; }

    // Interface: IGatewayGuild
    /// <inheritdoc/>
    public DateTimeOffset JoinedAt { get; private set; }

    /// <inheritdoc/>
    public bool IsLarge { get; private set; }

    /// <inheritdoc/>
    public bool IsUnavailable { get; private set; }

    /// <inheritdoc/>
    public int MemberCount { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IMember> Members
    {
        get
        {
            if (Client.CacheProvider.TryGetMembers(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedMember, IMember>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IMember>.Empty;
        }
    }

    /// <inheritdoc/>
    public GuildNsfwLevel NsfwLevel { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IGuildSticker> Stickers { get; private set; } = null!;

    /// <inheritdoc/>
    public bool IsBoostProgressBarEnabled { get; private set; }

    IReadOnlyDictionary<Snowflake, IGuildChannel> IGatewayGuild.Channels
    {
        get
        {
            if (Client.CacheProvider.TryGetChannels(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedGuildChannel, IGuildChannel>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IGuildChannel>.Empty;
        }
    }

    IReadOnlyDictionary<Snowflake, IVoiceState> IGatewayGuild.VoiceStates
    {
        get
        {
            if (Client.CacheProvider.TryGetVoiceStates(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedVoiceState, IVoiceState>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IVoiceState>.Empty;
        }
    }

    IReadOnlyDictionary<Snowflake, IPresence> IGatewayGuild.Presences
    {
        get
        {
            if (Client.CacheProvider.TryGetPresences(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedPresence, IPresence>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IPresence>.Empty;
        }
    }

    IReadOnlyDictionary<Snowflake, IStage> IGatewayGuild.Stages
    {
        get
        {
            if (Client.CacheProvider.TryGetStages(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedStage, IStage>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IStage>.Empty;
        }
    }

    IReadOnlyDictionary<Snowflake, IGuildEvent> IGatewayGuild.GuildEvents
    {
        get
        {
            if (Client.CacheProvider.TryGetGuildEvents(Id, out var cache, true))
                return new ReadOnlyUpcastingDictionary<Snowflake, CachedGuildEvent, IGuildEvent>(cache.ReadOnly());

            return ReadOnlyDictionary<Snowflake, IGuildEvent>.Empty;
        }
    }

    public CachedGuild(IGatewayClient client, GatewayGuildJsonModel model)
        : base(client, model.Id)
    {
        ApplicationId = model.ApplicationId;

        Update(model);
    }

    public void Update(GuildJsonModel model)
    {
        Name = model.Name;
        IconHash = model.Icon;
        SplashHash = model.Splash;

        if (model.DiscoverySplash.HasValue)
            DiscoverySplashHash = model.DiscoverySplash.Value;

        OwnerId = model.OwnerId;
        AfkChannelId = model.AfkChannelId;
        AfkTimeout = TimeSpan.FromSeconds(model.AfkTimeout);

        if (model.WidgetEnabled.HasValue)
            IsWidgetEnabled = model.WidgetEnabled.Value;

        if (model.WidgetChannelId.HasValue)
            WidgetChannelId = model.WidgetChannelId.Value;

        VerificationLevel = model.VerificationLevel;
        NotificationLevel = model.DefaultMessageNotifications;
        ContentFilterLevel = model.ExplicitContentFilter;
        SetEmojis(model.Emojis);
        SetStickers(model.Stickers);
        Features = model.Features;
        MfaLevel = model.MfaLevel;
        SystemChannelId = model.SystemChannelId;
        SystemChannelFlags = model.SystemChannelFlags;
        RulesChannelId = model.RulesChannelId;
        MaxPresenceCount = model.MaxPresences.GetValueOrDefault();
        MaxMemberCount = model.MaxMembers.GetValueOrNullable();
        VanityUrlCode = model.VanityUrlCode;
        Description = model.Description;
        BannerHash = model.Banner;
        BoostTier = model.PremiumTier;
        BoostingMemberCount = model.PremiumSubscriptionCount.GetValueOrNullable();
        PreferredLocale = Discord.Internal.GetLocale(model.PreferredLocale);
        PublicUpdatesChannelId = model.PublicUpdatesChannelId;
        MaxVideoMemberCount = model.MaxVideoChannelUsers.GetValueOrNullable();
        NsfwLevel = model.NsfwLevel;
        IsBoostProgressBarEnabled = model.PremiumProgressBarEnabled;
    }

    public void Update(GatewayGuildJsonModel model)
    {
        JoinedAt = model.JoinedAt;
        IsLarge = model.Large;

        if (model.Unavailable.HasValue)
            IsUnavailable = model.Unavailable.Value;

        MemberCount = model.MemberCount;

        Update(model as GuildJsonModel);
    }

    public void Update(UnavailableGuildJsonModel model)
    {
        if (model.Unavailable.HasValue)
            IsUnavailable = model.Unavailable.Value;
    }

    public void Update(GuildEmojisUpdateJsonModel model)
    {
        SetEmojis(model.Emojis);
    }

    public void Update(GuildStickersUpdateJsonModel model)
    {
        SetStickers(model.Stickers);
    }

    public void Update(GuildMemberAddJsonModel model)
    {
        MemberCount++;
    }

    public void Update(GuildMemberRemoveJsonModel model)
    {
        MemberCount--;
    }

    private void SetEmojis(EmojiJsonModel[] emojis)
    {
        Emojis = emojis.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.Id!.Value,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
            });
    }

    private void SetStickers(Optional<StickerJsonModel[]> stickers)
    {
        if (!stickers.HasValue)
            Stickers = ReadOnlyDictionary<Snowflake, IGuildSticker>.Empty;

        Stickers = stickers.Value.ToReadOnlyDictionary(Client,
            (model, _) => model.Id,
            (model, client) => new TransientGuildSticker(client, model) as IGuildSticker);
    }
}

namespace Disqord;

public readonly partial struct GuildFeatures
{
    /// <summary>
    ///     The guild can set an animated banner.
    /// </summary>
    public const string AnimatedBanner = "ANIMATED_BANNER";

    /// <summary>
    ///     The guild can set an animated icon.
    /// </summary>
    public const string AnimatedIcon = "ANIMATED_ICON";

    /// <summary>
    ///     The guild has set up auto moderation rules.
    /// </summary>
    public const string AutoModeration = "AUTO_MODERATION";

    /// <summary>
    ///     The guild can set a banner.
    /// </summary>
    public const string Banner = "BANNER";

    /// <summary>
    ///     The guild has access to commercial features.
    ///     E.g. creating store channels.
    /// </summary>
    public const string Commerce = "COMMERCE";

    /// <summary>
    ///     The guild is a community guild.
    /// </summary>
    public const string Community = "COMMUNITY";

    /// <summary>
    ///     The guild is a developer support server on the Discord App Directory.
    /// </summary>
    public const string DeveloperSupportServer = "DEVELOPER_SUPPORT_SERVER";

    /// <summary>
    ///     The guild appears in guild discovery.
    /// </summary>
    public const string Discoverable = "DISCOVERABLE";

    /// <summary>
    ///     The guild appears in guild directory.
    /// </summary>
    public const string Featurable = "FEATURABLE";

    /// <summary>
    ///     The guild can set an invite splash background.
    /// </summary>
    public const string InviteSplash = "INVITE_SPLASH";

    /// <summary>
    ///     The guild has member screening.
    /// </summary>
    public const string MemberVerificationGateEnabled = "MEMBER_VERIFICATION_GATE_ENABLED";

    /// <summary>
    ///     The guild can create announcement channels.
    /// </summary>
    public const string News = "NEWS";

    /// <summary>
    ///     The guild is partnered.
    /// </summary>
    public const string Partnered = "PARTNERED";

    /// <summary>
    ///     The guild can be previewed prior to joining it.
    /// </summary>
    public const string PreviewEnabled = "PREVIEW_ENABLED";

    /// <summary>
    ///     The guild has a vanity URL.
    /// </summary>
    public const string VanityUrl = "VANITY_URL";

    /// <summary>
    ///     The guild is verified.
    /// </summary>
    public const string Verified = "VERIFIED";

    /// <summary>
    ///     The guild can set a bitrate of <c>384</c>kbps in voice channels.
    /// </summary>
    public const string VipRegions = "VIP_REGIONS";

    /// <summary>
    ///     The guild has a welcome screen.
    /// </summary>
    public const string WelcomeScreenEnabled = "WELCOME_SCREEN_ENABLED";

    /// <summary>
    ///     The guild has ticketed events.
    /// </summary>
    public const string TicketedEventsEnabled = "TICKETED_EVENTS_ENABLED";

    /// <summary>
    ///     The guild has monetization enabled.
    /// </summary>
    public const string MonetizationEnabled = "MONETIZATION_ENABLED";

    /// <summary>
    ///     The guild has extra sticker slots.
    /// </summary>
    public const string MoreStickers = "MORE_STICKERS";

    /// <summary>
    ///     The guild has access to the three day archive time for threads.
    /// </summary>
    public const string ThreeDayThreadArchive = "THREE_DAY_THREAD_ARCHIVE";

    /// <summary>
    ///     The guild has access to the seven day archive time for threads.
    /// </summary>
    public const string SevenDayThreadArchive = "SEVEN_DAY_THREAD_ARCHIVE";

    /// <summary>
    ///     The guild can create private threads.
    /// </summary>
    public const string PrivateThreads = "PRIVATE_THREADS";

    /// <summary>
    ///     The guild can set role icons.
    /// </summary>
    public const string RoleIcons = "ROLE_ICONS";

    /// <summary>
    ///     The guild has access to guild tags.
    /// </summary>
    public const string GuildTags = "GUILD_TAGS";

    /// <summary>
    ///     The guild can set gradient colors on roles.
    /// </summary>
    public const string EnhancedRoleColors = "ENHANCED_ROLE_COLORS";

    /// <summary>
    ///     The guild has access to guest invites.
    /// </summary>
    public const string GuestsEnabled = "GUESTS_ENABLED";

    /// <summary>
    ///     The guild has extra soundboard sound slots.
    /// </summary>
    public const string MoreSoundboard = "MORE_SOUNDBOARD";

    /// <summary>
    ///     The guild has enabled monetization provisionally.
    /// </summary>
    public const string CreatorMonetizableProvisional = "CREATOR_MONETIZABLE_PROVISIONAL";

    /// <summary>
    ///     The guild has enabled the role subscription promo page.
    /// </summary>
    public const string CreatorStorePage = "CREATOR_STORE_PAGE";

    /// <summary>
    ///     The guild has paused invites, preventing new users from joining.
    /// </summary>
    public const string InvitesDisabled = "INVITES_DISABLED";

    /// <summary>
    ///     The guild has disabled alerts for join raids.
    /// </summary>
    public const string RaidAlertsDisabled = "RAID_ALERTS_DISABLED";

    /// <summary>
    ///     The guild is using the old application command permissions behavior.
    /// </summary>
    public const string ApplicationCommandPermissionsV2 = "APPLICATION_COMMAND_PERMISSIONS_V2";

    /// <summary>
    ///     The guild has a directory entry.
    /// </summary>
    public const string HasDirectoryEntry = "HAS_DIRECTORY_ENTRY";

    /// <summary>
    ///     The guild is a Student Hub.
    /// </summary>
    public const string Hub = "HUB";

    /// <summary>
    ///     The guild is linked to a Student Hub.
    /// </summary>
    public const string LinkedToHub = "LINKED_TO_HUB";

    /// <summary>
    ///     The guild has completed the pin permission migration.
    /// </summary>
    public const string PinPermissionMigrationComplete = "PIN_PERMISSION_MIGRATION_COMPLETE";

    /// <summary>
    ///     The guild has relay enabled.
    /// </summary>
    public const string RelayEnabled = "RELAY_ENABLED";

    /// <summary>
    ///     The guild has role subscriptions available for purchase.
    /// </summary>
    public const string RoleSubscriptionsAvailableForPurchase = "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE";

    /// <summary>
    ///     The guild has role subscriptions enabled.
    /// </summary>
    public const string RoleSubscriptionsEnabled = "ROLE_SUBSCRIPTIONS_ENABLED";

    /// <summary>
    ///     The guild has created soundboard sounds.
    /// </summary>
    public const string Soundboard = "SOUNDBOARD";
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord;

/// <summary>
///     Represents a utility wrapper around guild features.
/// </summary>
/// <example>
///     Checking whether a guild is discoverable.
///     <code language="csharp">
///     var isDiscoverable = guild.GetFeatures().IsDiscoverable;
///     </code>
/// </example>
public readonly partial struct GuildFeatures : IReadOnlyList<string>
{
    /// <summary>
    ///     Gets whether the guild can set an animated banner.
    /// </summary>
    public bool HasAnimatedBanner => Has(AnimatedBanner);

    /// <summary>
    ///     Gets whether the guild can set an animated icon.
    /// </summary>
    public bool HasAnimatedIcon => Has(AnimatedIcon);

    /// <summary>
    ///     Gets whether the guild has set up auto moderation rules.
    /// </summary>
    public bool HasAutoModeration => Has(AutoModeration);

    /// <summary>
    ///     Gets whether the guild can set a banner.
    /// </summary>
    public bool HasBanner => Has(Banner);

    /// <summary>
    ///     Gets whether the guild has access to commercial features.
    ///     E.g. creating store channels.
    /// </summary>
    public bool HasCommerce => Has(Commerce);

    /// <summary>
    ///     Gets whether the guild is a community guild.
    /// </summary>
    public bool HasCommunity => Has(Community);

    /// <summary>
    ///     Gets whether the guild appears in guild discovery.
    /// </summary>
    public bool IsDiscoverable => Has(Discoverable);

    /// <summary>
    ///     Gets whether the guild appears in guild directory.
    /// </summary>
    public bool IsFeaturable => Has(Featurable);

    /// <summary>
    ///     Gets whether the guild can set an invite splash background.
    /// </summary>
    public bool HasInviteSplash => Has(InviteSplash);

    /// <summary>
    ///     Gets whether the guild has member screening.
    /// </summary>
    public bool HasMemberVerificationGateEnabled => Has(MemberVerificationGateEnabled);

    /// <summary>
    ///     Gets whether the guild can create announcement channels.
    /// </summary>
    public bool HasNews => Has(News);

    /// <summary>
    ///     Gets whether the guild is partnered.
    /// </summary>
    public bool IsPartnered => Has(Partnered);

    /// <summary>
    ///     Gets whether the guild can be previewed prior to joining it.
    /// </summary>
    public bool HasPreviewEnabled => Has(PreviewEnabled);

    /// <summary>
    ///     Gets whether the guild has a vanity URL.
    /// </summary>
    public bool HasVanityUrl => Has(VanityUrl);

    /// <summary>
    ///     Gets whether the guild is verified.
    /// </summary>
    public bool IsVerified => Has(Verified);

    /// <summary>
    ///     Gets whether the guild can set a bitrate of <c>384</c>kbps in voice channels.
    /// </summary>
    public bool HasVipRegions => Has(VipRegions);

    /// <summary>
    ///     Gets whether the guild has a welcome screen.
    /// </summary>
    public bool HasWelcomeScreenEnabled => Has(WelcomeScreenEnabled);

    /// <summary>
    ///     Gets whether the guild has ticketed events.
    /// </summary>
    public bool HasTicketedEventsEnabled => Has(TicketedEventsEnabled);

    /// <summary>
    ///     Gets whether the guild has monetization enabled.
    /// </summary>
    public bool HasMonetizationEnabled => Has(MonetizationEnabled);

    /// <summary>
    ///     Gets whether the guild has extra sticker slots.
    /// </summary>
    public bool HasMoreStickers => Has(MoreStickers);

    /// <summary>
    ///     Gets whether the guild has access to the three day archive time for threads.
    /// </summary>
    public bool HasThreeDayThreadArchive => Has(ThreeDayThreadArchive);

    /// <summary>
    ///     Gets whether the guild has access to the seven day archive time for threads.
    /// </summary>
    public bool HasSevenDayThreadArchive => Has(SevenDayThreadArchive);

    /// <summary>
    ///     Gets whether the guild can create private threads.
    /// </summary>
    public bool HasPrivateThreads => Has(PrivateThreads);

    /// <summary>
    ///     Gets whether the guild can set role icons.
    /// </summary>
    public bool HasRoleIcons => Has(RoleIcons);

    private readonly IReadOnlyList<string> _features;

    public GuildFeatures(IReadOnlyList<string> features)
    {
        _features = features;
    }

    public bool Has(string feature)
        => _features != null && _features.Contains(feature);

    public int Count => _features?.Count ?? 0;

    public string this[int index] => _features?[index] ?? throw new IndexOutOfRangeException();

    public IEnumerator<string> GetEnumerator()
    {
        return (_features ?? Enumerable.Empty<string>()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string? ToString()
    {
        return _features != null ? string.Join(", ", _features) : null;
    }
}
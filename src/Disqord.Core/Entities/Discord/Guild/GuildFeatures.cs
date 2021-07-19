using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public readonly struct GuildFeatures : IReadOnlyList<string>
    {
        /// <summary>
        ///     The guild can set an animated icon.
        /// </summary>
        public bool AnimatedIcon => Has("ANIMATED_ICON");

        /// <summary>
        ///     The guild can set a banner.
        /// </summary>
        public bool Banner => Has("BANNER");

        /// <summary>
        ///     The guild has access to commercial features.
        ///     E.g. creating store channels.
        /// </summary>
        public bool Commerce => Has("COMMERCE");

        /// <summary>
        ///     The guild is a community guild.
        /// </summary>
        public bool Community => Has("COMMUNITY");

        /// <summary>
        ///     The guild appears in guild discovery.
        /// </summary>
        public bool Discoverable => Has("DISCOVERABLE");

        /// <summary>
        ///     The guild cannot be discoverable.
        /// </summary>
        public bool DiscoverableDisabled => Has("DISCOVERABLE_DISABLED");

        /// <summary>
        ///     The guild appears in guild directory.
        /// </summary>
        public bool Featurable => Has("FEATURABLE");

        /// <summary>
        ///     The guild can set an invite splash background.
        /// </summary>
        public bool InviteSplash => Has("INVITE_SPLASH");

        /// <summary>
        ///     The guild has member screening.
        /// </summary>
        public bool MemberVerificationGateEnabled => Has("MEMBER_VERIFICATION_GATE_ENABLED");

        /// <summary>
        ///     The guild can create announcement channels.
        /// </summary>
        public bool News => Has("NEWS");

        /// <summary>
        ///     The guild is partnered.
        /// </summary>
        public bool Partnered => Has("PARTNERED");

        /// <summary>
        ///     The guild can be previewed prior to joining it.
        /// </summary>
        public bool PreviewEnabled => Has("PREVIEW_ENABLED");

        /// <summary>
        ///     The guild has a vanity URL.
        /// </summary>
        public bool VanityUrl => Has("VANITY_URL");

        /// <summary>
        ///     The guild is verified.
        /// </summary>
        public bool Verified => Has("VERIFIED");

        /// <summary>
        ///     The guild can set a bitrate of <c>384</c>kbps in voice channels.
        /// </summary>
        public bool VipRegions => Has("VIP_REGIONS");

        /// <summary>
        ///     The guild has a welcome screen.
        /// </summary>
        public bool WelcomeScreenEnabled => Has("WELCOME_SCREEN_ENABLED");

        /// <summary>
        ///     The guild has ticketed events.
        /// </summary>
        public bool TicketedEventsEnabled => Has("TICKETED_EVENTS_ENABLED");

        /// <summary>
        ///     The guild has monetization enabled.
        /// </summary>
        public bool MonetizationEnabled => Has("MONETIZATION_ENABLED");

        /// <summary>
        ///     The guild has extra sticker slots.
        /// </summary>
        public bool MoreStickers => Has("MORE_STICKERS");

        /// <summary>
        ///     The guild has access to the three day archive time for threads.
        /// </summary>
        public bool ThreeDayThreadArchive => Has("THREE_DAY_THREAD_ARCHIVE");

        /// <summary>
        ///     The guild has access to the seven day archive time for threads.
        /// </summary>
        public bool SevenDayThreadArchive => Has("SEVEN_DAY_THREAD_ARCHIVE");

        /// <summary>
        ///     The guild can create private threads.
        /// </summary>
        public bool PrivateThreads => Has("PRIVATE_THREADS");

        private readonly string[] _features;

        public GuildFeatures(string[] features)
        {
            _features = features;
        }

        public bool Has(string feature)
            => _features != null && Array.IndexOf(_features, feature) != -1;

        public int Count => _features?.Length ?? 0;

        public string this[int index] => _features?[index] ?? throw new IndexOutOfRangeException();

        public IEnumerator<string> GetEnumerator()
            => (_features ?? Enumerable.Empty<string>()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public override string ToString()
            => _features != null ? string.Join(", ", _features) : null;
    }
}

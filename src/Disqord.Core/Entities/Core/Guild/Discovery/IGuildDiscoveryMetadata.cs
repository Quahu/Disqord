using System;
using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild's discovery settings.
    /// </summary>
    public interface IGuildDiscoveryMetadata : IGuildEntity
    {
        /// <summary>
        ///     Gets the ID of the primary discovery category set for the guild.
        /// </summary>
        int PrimaryCategoryId { get; }

        /// <summary>
        ///     Gets the discovery search keywords for the guild.
        /// </summary>
        IReadOnlyList<string> Keywords { get; }

        /// <summary>
        ///     Gets whether the guild is discoverable from emojis sent in messages.
        /// </summary>
        bool HasEmojis { get; }

        /// <summary>
        ///     Gets when the guild's partner application was accepted or denied.
        /// </summary>
        DateTimeOffset? PartnershipChangedAt { get; }

        /// <summary>
        ///     Gets when the guild applied for partnership if it has a pending request.
        /// </summary>
        DateTimeOffset? PartnershipAppliedAt { get; }

        /// <summary>
        ///     Gets up to 5 discovery subcategory IDs for the guild.
        /// </summary>
        IReadOnlyList<int> CategoryIds { get; }
    }
}
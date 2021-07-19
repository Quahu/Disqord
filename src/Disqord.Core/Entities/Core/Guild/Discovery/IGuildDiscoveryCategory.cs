using System.Collections.Generic;
using System.Globalization;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild's discovery category.
    /// </summary>
    public interface IGuildDiscoveryCategory : INamable
    {
        /// <summary>
        ///     Gets the ID of this category.
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     Gets whether this category can be set as the primary category.
        /// </summary>
        bool IsPrimary { get; }
        
        /// <summary>
        ///     Gets the name of this category in other languages.
        /// </summary>
        IReadOnlyDictionary<CultureInfo, string> LocalizedNames { get; }
    }
}
using System.Collections.Generic;

namespace Disqord.Bot
{
    public class DefaultPrefixProviderConfiguration
    {
        /// <summary>
        ///     Gets or sets whether the <see cref="DefaultPrefixProvider.Prefixes"/> collection is mutable or read-only.
        ///     Defaults to <see langword="false"/> for performance reasons.
        /// </summary>
        /// <remarks>
        ///     Mutability comes at the price of having to copy the prefix set to an array on every message checked for prefixes.
        ///     A custom <see cref="IPrefixProvider"/> should be implemented with caching systems in order to prevent that.
        /// </remarks>
        public virtual bool AllowsMutability { get; set; } = false;

        /// <summary>
        ///     Gets or sets the prefixes to populate the provider with.
        /// </summary>
        public virtual IEnumerable<IPrefix> Prefixes { get; set; }
    }
}
using System.Collections.Generic;

namespace Disqord.Bot
{
    public class DefaultPrefixProviderConfiguration
    {
        /// <summary>
        ///     Gets or sets the prefixes to populate the provider with.
        /// </summary>
        public virtual IEnumerable<IPrefix> Prefixes { get; set; }
    }
}

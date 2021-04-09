using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Collections.Synchronized;
using Disqord.Gateway;
using Microsoft.Extensions.Options;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents the default implementation of an <see cref="IPrefixProvider"/>.
    ///     Returns the same set of prefixes for all messages and is not mutable by default.
    /// </summary>
    public class DefaultPrefixProvider : IPrefixProvider
    {
        /// <summary>
        ///     Gets the prefixes set of this <see cref="DefaultPrefixProvider"/>.
        /// </summary>
        /// <remarks>
        ///     This set is read-only by default, unless specified otherwise by <see cref="DefaultPrefixProviderConfiguration.AllowsMutability"/>.
        /// </remarks>
        public ISet<IPrefix> Prefixes { get; }

        /// <summary>
        ///     Instantiates a new <see cref="DefaultPrefixProvider"/>.
        /// </summary>
        /// <param name="options"> The configuration options. </param>
        public DefaultPrefixProvider(
            IOptions<DefaultPrefixProviderConfiguration> options)
        {
            var configuration = options.Value;
            var prefixes = configuration.Prefixes ?? new IPrefix[0];
            if (configuration.AllowsMutability)
            {
                // Mutability comes at the price of ToArray()ing the set on every message.
                Prefixes = new SynchronizedHashSet<IPrefix>(prefixes);
            }
            else
            {
                Prefixes = new ReadOnlySet<IPrefix>(new HashSet<IPrefix>(prefixes));
            }
        }

        /// <inheritdoc/>
        public ValueTask<IEnumerable<IPrefix>> GetPrefixesAsync(IGatewayUserMessage message)
            => new(Prefixes);
    }
}

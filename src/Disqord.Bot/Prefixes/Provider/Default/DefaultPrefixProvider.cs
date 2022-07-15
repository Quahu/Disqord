using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Disqord.Gateway;
using Microsoft.Extensions.Options;

namespace Disqord.Bot.Commands.Text;

/// <summary>
///     Represents the default implementation of an <see cref="IPrefixProvider"/>.
///     Returns <see cref="Prefixes"/> for all messages.
/// </summary>
public class DefaultPrefixProvider : IPrefixProvider
{
    /// <summary>
    ///     Gets or sets the immutable set of prefixes of this <see cref="DefaultPrefixProvider"/>.
    /// </summary>
    public IImmutableSet<IPrefix> Prefixes { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultPrefixProvider"/>.
    /// </summary>
    /// <param name="options"> The configuration options. </param>
    public DefaultPrefixProvider(
        IOptions<DefaultPrefixProviderConfiguration> options)
    {
        var configuration = options.Value;
        Prefixes = configuration.Prefixes?.ToImmutableHashSet() ?? ImmutableHashSet<IPrefix>.Empty;
    }

    /// <inheritdoc/>
    public ValueTask<IEnumerable<IPrefix>?> GetPrefixesAsync(IGatewayUserMessage message)
        => new(Prefixes);
}

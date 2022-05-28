using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Bot.Commands.Text;

/// <summary>
///     Represents a type that that provides prefixes based on the received message.
/// </summary>
public interface IPrefixProvider
{
    /// <summary>
    ///     Gets prefixes for the given message.
    /// </summary>
    /// <param name="message"> The message to get prefixes for. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> wrapping an enumerable of prefixes.
    /// </returns>
    ValueTask<IEnumerable<IPrefix>?> GetPrefixesAsync(IGatewayUserMessage message);
}

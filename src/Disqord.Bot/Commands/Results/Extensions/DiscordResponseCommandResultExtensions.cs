using System;

namespace Disqord.Bot.Commands;

public static class DiscordResponseCommandResultExtensions
{
    /// <summary>
    ///     Wraps the <see cref="DiscordResponseCommandResult"/> in a new result which will
    ///     delete the sent response after the specified delay.
    /// </summary>
    /// <param name="result"> The result to wrap. </param>
    /// <param name="delay"> The delay after which the response should be deleted. </param>
    /// <returns>
    ///     A <see cref="DiscordTemporaryResponseCommandResult"/>.
    /// </returns>
    public static DiscordTemporaryResponseCommandResult DeleteAfter(this DiscordResponseCommandResult result, TimeSpan delay)
    {
        return new(result, delay);
    }
}

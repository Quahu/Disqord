using System;
using Disqord.Gateway;

namespace Disqord.Bot;

/// <summary>
///     Represents a message prefix used for command execution.
/// </summary>
public interface IPrefix
{
    /// <summary>
    ///     Attempts to find this prefix in the specified message and return the substring ready for execution.
    /// </summary>
    /// <param name="message"> The message to check for the prefix in. </param>
    /// <param name="output"> The output substring ready for execution. </param>
    /// <returns>
    ///     <see langword="true"/>, if the prefix was found.
    /// </returns>
    bool TryFind(IGatewayUserMessage message, out ReadOnlyMemory<char> output);
}

using System;

namespace Disqord
{
    /// <summary>
    ///     Represents the flags of a response to an interaction.
    /// </summary>
    [Flags]
    public enum InteractionResponseFlag
    {
        /// <summary>
        ///     The response has no flags.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The response is ephemeral, i.e. only visible to the interaction author.
        /// </summary>
        Ephemeral = 64
    }
}

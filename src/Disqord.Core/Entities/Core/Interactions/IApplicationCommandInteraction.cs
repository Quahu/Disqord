using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command interaction
    /// </summary>
    public interface IApplicationCommandInteraction : IInteraction, INamable
    {
        /// <summary>
        ///     Gets the ID of the command of this interaction
        /// </summary>
        Snowflake CommandId { get; }

        /// <summary>
        ///     Gets the resolved data of this interaction
        /// </summary>
        IApplicationCommandInteractionResolvedData Resolved { get; }

        /// <summary>
        ///     Gets the options data of this interaction
        /// </summary>
        IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }
    }
}
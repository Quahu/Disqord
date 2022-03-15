using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a modal submit interaction
    /// </summary>
    public interface IModalSubmitInteraction : IIdentifiableInteraction
    {
        /// <summary>
        ///     Gets the components of this interaction.
        /// </summary>
        IReadOnlyList<IComponent> Components { get; }
    }
}

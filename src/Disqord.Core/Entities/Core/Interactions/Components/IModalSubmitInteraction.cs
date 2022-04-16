using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a modal being submitted.
    /// </summary>
    public interface IModalSubmitInteraction : IInteraction, ICustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets the components of this interaction.
        /// </summary>
        IReadOnlyList<IComponent> Components { get; }
    }
}

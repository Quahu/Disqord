using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a modal being submitted.
    /// </summary>
    public interface IModalSubmitInteraction : IInteraction, ICustomIdentifiable
    {
        /// <summary>
        ///     Gets the components of this interaction.
        /// </summary>
        IReadOnlyList<IComponent> Components { get; }
    }
}

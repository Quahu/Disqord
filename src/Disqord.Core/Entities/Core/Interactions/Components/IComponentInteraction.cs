using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a component interaction
    /// </summary>
    public interface IComponentInteraction : IInteraction, ICustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets the component type of this interaction.
        /// </summary>
        ComponentType ComponentType { get; }

        /// <summary>
        ///     Gets the selected values of this interaction if <see cref="ComponentType"/> is <see cref="Disqord.ComponentType.Selection"/>.
        /// </summary>
        IReadOnlyList<string> SelectedValues { get; }

        /// <summary>
        ///     Gets the message of this interaction.
        /// </summary>
        IUserMessage Message { get; }
    }
}

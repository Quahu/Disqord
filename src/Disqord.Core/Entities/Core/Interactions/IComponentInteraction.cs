using System.Collections.Generic;

namespace Disqord
{
    public interface IComponentInteraction : IInteraction
    {
        /// <summary>
        ///     Gets the custom ID of the component of this interaction.
        /// </summary>
        string CustomId { get; }

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

using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a selection message component.
    /// </summary>
    public interface ISelectionComponent : IComponent
    {
        /// <summary>
        ///     Gets the custom ID of this selection component.
        /// </summary>
        string CustomId { get; }

        /// <summary>
        ///     Gets the placeholder text of this selection component.
        ///     Returns <see langword="null"/> if not set.
        /// </summary>
        string Placeholder { get; }

        /// <summary>
        ///     Gets the minimum amount of options that must be selected at once of this selection component.
        /// </summary>
        int MinimimumSelectedOptions { get; }

        /// <summary>
        ///     Gets the maximum amount of options that can be selected at once of this selection component.
        /// </summary>
        int MaximumSelectedOptions { get; }

        /// <summary>
        ///     Gets the selectable options of this selection component.
        /// </summary>
        IReadOnlyList<ISelectionComponentOption> Options { get; }

        /// <summary>
        ///     Gets whether this selection component is disabled.
        /// </summary>
        bool IsDisabled { get; }
    }
}

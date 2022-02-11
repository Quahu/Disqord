using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a provided value for a slash command option.
    /// </summary>
    public interface IAutoCompleteInteractionOption
    {
        /// <summary>
        ///     Gets the name of this option.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the type of this option.
        /// </summary>
        SlashCommandOptionType Type { get; }

        /// <summary>
        ///     Gets the value of this option.
        /// </summary>
        object Value { get; }

        /// <summary>
        ///     Gets the nested options of this option.
        /// </summary>
        IReadOnlyDictionary<string, IAutoCompleteInteractionOption> Options { get; }

        /// <summary>
        ///     Gets whether this option is currently focused by the user typing out the slash command,
        ///     i.e. the option the auto-complete should provide the choices for.
        /// </summary>
        bool IsFocused { get; }
    }
}

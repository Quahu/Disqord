using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a slash command.
    /// </summary>
    public interface ISlashCommand : IApplicationCommand
    {
        /// <summary>
        ///     Gets the description of this slash command.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the options of this slash command.
        /// </summary>
        IReadOnlyList<ISlashCommandOption> Options { get; }
    }
}

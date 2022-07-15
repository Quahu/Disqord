using System.Collections.Generic;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents command cache changes.
/// </summary>
public interface IApplicationCommandCacheChanges
{
    /// <summary>
    ///     Gets whether there are any cache changes.
    /// </summary>
    bool Any { get; }

    /// <summary>
    ///     Gets whether the changes are the initial changes,
    ///     i.e. whether there was no cached data priorly.
    /// </summary>
    bool AreInitial { get; }

    /// <summary>
    ///     Gets the commands that were unchanged.
    /// </summary>
    IReadOnlyList<LocalApplicationCommand> UnchangedCommands { get; }

    /// <summary>
    ///     Gets the commands that were created.
    /// </summary>
    IReadOnlyList<LocalApplicationCommand> CreatedCommands { get; }

    /// <summary>
    ///     Gets the commands that were modified.
    /// </summary>
    IReadOnlyDictionary<Snowflake, LocalApplicationCommand> ModifiedCommands { get; }

    /// <summary>
    ///     Gets the IDs of commands that were deleted.
    /// </summary>
    IReadOnlyList<Snowflake> DeletedCommandIds { get; }
}

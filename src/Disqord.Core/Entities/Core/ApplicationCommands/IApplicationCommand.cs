using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an application command.
/// </summary>
public interface IApplicationCommand : ISnowflakeEntity, IPossiblyGuildEntity, INamableEntity, IJsonUpdatable<ApplicationCommandJsonModel>
{
    /// <summary>
    ///     Gets the type of this command.
    /// </summary>
    ApplicationCommandType Type { get; }

    /// <summary>
    ///     Gets the ID of the application of this command.
    /// </summary>
    Snowflake ApplicationId { get; }

    /// <summary>
    ///     Gets the name localizations of this command.
    /// </summary>
    /// <remarks>
    ///     Might be empty if retrieved using bulk application command fetch endpoints.
    /// </remarks>
    IReadOnlyDictionary<CultureInfo, string> NameLocalizations { get; }

    /// <summary>
    ///     Gets the default required member permissions of this command.
    /// </summary>
    Permissions? DefaultRequiredMemberPermissions { get; }

    /// <summary>
    ///     Gets whether this command is enabled in private channels.
    /// </summary>
    bool IsEnabledInPrivateChannels { get; }

    /// <summary>
    ///     Gets whether this application command is enabled by default.
    /// </summary>
    bool IsEnabledByDefault { get; }

    /// <summary>
    ///     Gets the auto-incrementing version of this command.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Snowflake"/> is automatically incremented by Discord each time the command is updated.
    /// </remarks>
    Snowflake Version { get; }
}
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents role connection metadata.
/// </summary>
public interface IApplicationRoleConnectionMetadata : INamableEntity, IJsonUpdatable<ApplicationRoleConnectionMetadataJsonModel>
{
    /// <summary>
    ///     Gets the type of this metadata.
    /// </summary>
    ApplicationRoleConnectionMetadataType Type { get; }

    /// <summary>
    ///     Gets the key of this metadata.
    /// </summary>
    string Key { get; }

    /// <summary>
    ///     Gets the name localizations of this metadata.
    /// </summary>
    IReadOnlyDictionary<string, string> NameLocalizations { get; }

    /// <summary>
    ///     Gets the description of this metadata.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the description localizations of this metadata.
    /// </summary>
    IReadOnlyDictionary<string, string> DescriptionLocalizations { get; }
}

using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.OAuth2;

/// <summary>
///     Represents a role connection.
/// </summary>
public interface IApplicationRoleConnection : IEntity, IJsonUpdatable<ApplicationRoleConnectionJsonModel>
{
    /// <summary>
    ///     Gets the name of the platform of this role connection.
    /// </summary>
    string? PlatformName { get; }

    /// <summary>
    ///     Gets the username of the user on the platform of this role connection.
    /// </summary>
    string? PlatformUsername { get; }

    /// <summary>
    ///     Gets the <see cref="IApplicationRoleConnectionMetadata"/> keys mapped to their values.
    /// </summary>
    IReadOnlyDictionary<string, string> Metadata { get; }
}

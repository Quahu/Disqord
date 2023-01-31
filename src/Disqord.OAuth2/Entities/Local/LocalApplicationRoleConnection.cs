using System.Collections.Generic;
using Qommon;
using Qommon.Collections;

namespace Disqord.OAuth2;

public class LocalApplicationRoleConnection : ILocalConstruct<LocalApplicationRoleConnection>
{
    /// <summary>
    ///     Gets or sets the name of the platform of this role connection.
    /// </summary>
    public Optional<string> PlatformName { get; set; }

    /// <summary>
    ///     Gets or sets the username of the user on the platform of this role connection.
    /// </summary>
    public Optional<string> PlatformUsername { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="IApplicationRoleConnectionMetadata"/> keys mapped to their values.
    /// </summary>
    public Optional<IDictionary<string, string>> Metadata { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationRoleConnection"/>.
    /// </summary>
    public LocalApplicationRoleConnection()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationRoleConnection"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalApplicationRoleConnection(LocalApplicationRoleConnection other)
    {
        Guard.IsNotNull(other);

        PlatformName = other.PlatformName;
        PlatformUsername = other.PlatformUsername;
        Metadata = Optional.Convert(other.Metadata, localizations => localizations.ToDictionary() as IDictionary<string, string>);
    }

    /// <inheritdoc/>
    public LocalApplicationRoleConnection Clone()
    {
        return new(this);
    }
}

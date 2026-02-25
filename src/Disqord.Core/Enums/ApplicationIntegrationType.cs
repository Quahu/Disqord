namespace Disqord;

/// <summary>
///     Represents where an application can be installed.
/// </summary>
public enum ApplicationIntegrationType
{
    /// <summary>
    ///     The application is installable to guilds.
    /// </summary>
    GuildInstall = 0,

    /// <summary>
    ///     The application is installable to users.
    /// </summary>
    UserInstall = 1,
}

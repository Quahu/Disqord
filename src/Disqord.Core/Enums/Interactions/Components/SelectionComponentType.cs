namespace Disqord;

/// <summary>
///     Represents the type of values in a selection component.
/// </summary>
public enum SelectionComponentType : byte
{
    /// <summary>
    ///     The values are pre-defined string choices.
    /// </summary>
    String = 3,

    /// <summary>
    ///     The values are users.
    /// </summary>
    User = 5,

    /// <summary>
    ///     The values are roles.
    /// </summary>
    Role = 6,

    /// <summary>
    ///     The values are entities that are mentionable within the Discord client
    ///     such as users, roles, or channels.
    /// </summary>
    Mentionable = 7,

    /// <summary>
    ///     The values are channels.
    /// </summary>
    Channel = 8
}

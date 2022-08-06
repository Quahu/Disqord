namespace Disqord;

/// <summary>
///     Represents the type of a slash command option.
/// </summary>
public enum SlashCommandOptionType
{
    /// <summary>
    ///     The option is a subcommand.
    /// </summary>
    Subcommand = 1,

    /// <summary>
    ///     The option is a subcommand group.
    /// </summary>
    SubcommandGroup = 2,

    /// <summary>
    ///     The option is a string parameter.
    /// </summary>
    String = 3,

    /// <summary>
    ///     The option is an integral numeric parameter.
    /// </summary>
    Integer = 4,

    /// <summary>
    ///     The option is a boolean parameter.
    /// </summary>
    Boolean = 5,

    /// <summary>
    ///     The option is a user parameter.
    /// </summary>
    User = 6,

    /// <summary>
    ///     The option is a channel parameter.
    /// </summary>
    Channel = 7,

    /// <summary>
    ///     The option is a role parameter.
    /// </summary>
    Role = 8,

    /// <summary>
    ///     The option is a mentionable parameter,
    ///     i.e. any entity that is mentionable within the Discord client
    ///     such as users, channels, roles, etc.
    /// </summary>
    Mentionable = 9,

    /// <summary>
    ///     The option is a floating-point numeric parameter.
    /// </summary>
    Number = 10,

    /// <summary>
    ///     The option is an attachment.
    /// </summary>
    Attachment = 11
}

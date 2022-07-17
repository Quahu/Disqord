namespace Disqord;

/// <summary>
///     Represents an application command interaction.
/// </summary>
public interface IApplicationCommandInteraction : IUserInteraction
{
    /// <summary>
    ///     Gets the ID of the application command of this interaction.
    /// </summary>
    Snowflake CommandId { get; }

    /// <summary>
    ///     Gets the name of the application command of this interaction.
    /// </summary>
    string CommandName { get; }

    /// <summary>
    ///     Gets the type of the application command of this interaction.
    /// </summary>
    ApplicationCommandType CommandType { get; }

    /// <summary>
    ///     Gets the ID of the guild in which the application command of this interaction is registered in.
    /// </summary>
    Snowflake? CommandGuildId { get; }

    /// <summary>
    ///     Gets the resolved entities of this interaction.
    /// </summary>
    IApplicationCommandInteractionEntities Entities { get; }
}
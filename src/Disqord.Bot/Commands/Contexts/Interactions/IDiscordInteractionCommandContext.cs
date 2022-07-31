namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents an interaction command execution context.
/// </summary>
public interface IDiscordInteractionCommandContext : IDiscordCommandContext
{
    /// <summary>
    ///     Gets the interaction that triggered this command execution.
    /// </summary>
    IUserInteraction Interaction { get; }

    /// <summary>
    ///     Gets the author's permissions in the context channel of this command execution.
    /// </summary>
    Permissions AuthorPermissions => Interaction.AuthorPermissions;

    /// <summary>
    ///     Gets the application's permissions in the context channel of this command execution.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="IUserInteraction.ApplicationPermissions"/>
    /// </remarks>
    Permissions ApplicationPermissions => Interaction.ApplicationPermissions;
}

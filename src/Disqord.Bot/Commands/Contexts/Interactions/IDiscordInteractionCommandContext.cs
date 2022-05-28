namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents an interaction command execution context.
/// </summary>
public interface IDiscordInteractionCommandContext : IDiscordCommandContext
{
    /// <summary>
    ///     Gets the interaction that triggered this command execution.
    /// </summary>
    IInteraction Interaction { get; }

    /// <summary>
    ///     Gets the author's permissions in the context channel of this command execution.
    /// </summary>
    Permission AuthorPermissions => Interaction.AuthorPermissions;

    IUser IDiscordCommandContext.Author => Interaction.Author;
}

namespace Disqord;

/// <summary>
///     Represents a context menu interaction.
/// </summary>
public interface IContextMenuInteraction : IApplicationCommandInteraction
{
    /// <summary>
    ///     Gets the ID of the target entity of this interaction.
    /// </summary>
    Snowflake TargetId { get; }
}
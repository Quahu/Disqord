namespace Disqord;

/// <summary>
///     Represents an interaction triggered by a user with resolved entities.
/// </summary>
public interface IEntityInteraction : IUserInteraction
{
    /// <summary>
    ///     Gets the resolved entities of this interaction.
    /// </summary>
    IInteractionEntities Entities { get; }
}

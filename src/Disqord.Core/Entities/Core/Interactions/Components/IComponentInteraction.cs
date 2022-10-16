namespace Disqord;

/// <summary>
///     Represents a component interaction.
/// </summary>
public interface IComponentInteraction : IUserInteraction, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the component type of this interaction.
    /// </summary>
    ComponentType ComponentType { get; }

    /// <summary>
    ///     Gets the message of this interaction.
    /// </summary>
    IUserMessage Message { get; }
}

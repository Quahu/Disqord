using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a message component.
/// </summary>
public interface IComponent : IJsonUpdatable<BaseComponentJsonModel>
{
    /// <summary>
    ///     Gets the ID of this component.
    /// </summary>
    int Id { get; }

    /// <summary>
    ///     Gets the type of this component.
    /// </summary>
    ComponentType Type { get; }
}

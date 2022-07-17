using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a message component.
/// </summary>
public interface IComponent : IJsonUpdatable<ComponentJsonModel>
{
    /// <summary>
    ///     Gets the type of this component.
    /// </summary>
    ComponentType Type { get; }
}
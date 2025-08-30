using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a Discord component.
/// </summary>
/// <seealso cref="IComponent"/>
/// <seealso cref="IModalComponent"/>
public interface IBaseComponent : IJsonUpdatable<BaseComponentJsonModel>
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

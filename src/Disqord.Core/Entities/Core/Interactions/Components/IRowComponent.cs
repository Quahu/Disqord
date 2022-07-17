using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a row component which is a parent to multiple child components.
/// </summary>
public interface IRowComponent : IComponent, IEnumerable<IComponent>
{
    /// <summary>
    ///     Gets the child components of this row component.
    /// </summary>
    IReadOnlyList<IComponent> Components { get; }
}
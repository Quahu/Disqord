namespace Disqord;

/// <summary>
///     Represents a default value of a selection message component.
/// </summary>
public interface IDefaultSelectionValue: IIdentifiableEntity
{
    /// <summary>
    ///     Gets the type of this default value.
    /// </summary>
    DefaultSelectionValueType Type { get; }
}

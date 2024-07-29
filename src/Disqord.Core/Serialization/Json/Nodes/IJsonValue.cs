namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON value node, i.e. a single JSON value.
/// </summary>
public interface IJsonValue : IJsonNode
{
    /// <summary>
    ///     Gets the value of this JSON node.
    /// </summary>
    /// <typeparam name="T"> The type of the value. </typeparam>
    T? GetValue<T>();
}

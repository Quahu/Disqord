namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON node.
/// </summary>
public interface IJsonNode
{
    /// <summary>
    ///     Converts this JSON node to the given type.
    /// </summary>
    /// <typeparam name="T"> The type to convert to. </typeparam>
    /// <returns>
    ///     The converted type.
    /// </returns>
    T? ToType<T>();

    /// <summary>
    ///     Converts this node into a JSON string using the specified formatting.
    /// </summary>
    /// <param name="formatting"> The JSON formatting. </param>
    /// <returns>
    ///     The JSON string representation of this node.
    /// </returns>
    string ToJsonString(JsonFormatting formatting);
}

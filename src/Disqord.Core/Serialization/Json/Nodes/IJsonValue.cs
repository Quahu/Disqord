namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON value node, i.e. a single JSON value.
/// </summary>
public interface IJsonValue : IJsonNode
{
    /// <summary>
    ///     Gets the type of this JSON value.
    /// </summary>
    JsonValueType Type { get; }
}

namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON value node, i.e. a single JSON value.
/// </summary>
public interface IJsonValue : IJsonNode
{
    /// <summary>
    ///     Gets the value of this JSON node.
    /// </summary>
    object? Value { get; set; }
}
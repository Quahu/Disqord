using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON value node.
///     Wraps a <see cref="JsonValue"/>.
/// </summary>
[DebuggerDisplay($"{nameof(Value)}")]
public class SystemJsonValue : SystemJsonNode, IJsonValue
{
    /// <inheritdoc cref="SystemJsonNode.Node"/>
    public new JsonValue Node => (base.Node as JsonValue)!;

    private object? Value => GetValue<object>();

    public SystemJsonValue(JsonValue value, JsonSerializerOptions options)
        : base(value, options)
    { }

    /// <inheritdoc/>
    public T? GetValue<T>()
    {
        return Node.Deserialize<T>(Options);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Node.ToString();
    }
}

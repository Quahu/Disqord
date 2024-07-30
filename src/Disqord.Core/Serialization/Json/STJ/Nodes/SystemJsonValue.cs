using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON value node.
///     Wraps a <see cref="JsonValue"/>.
/// </summary>
[DebuggerDisplay($"{nameof(Value)}")]
internal sealed class SystemJsonValue : SystemJsonNode, IJsonValue
{
    /// <inheritdoc cref="SystemJsonNode.Node"/>
    public new JsonValue Node => (base.Node as JsonValue)!;

    private object? Value => ToType<object>();

    internal SystemJsonValue(JsonValue value, JsonSerializerOptions options)
        : base(value, options)
    { }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return Value?.ToString();
    }
}

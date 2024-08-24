using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON value node.
///     Wraps a <see cref="JsonValue"/>.
/// </summary>
[DebuggerDisplay($"{nameof(DebuggerDisplay)}")]
internal sealed class SystemJsonValue : SystemJsonNode, IJsonValue
{
    internal SystemJsonValue(JsonValue value, JsonSerializerOptions options)
        : base(value, options)
    { }

    private string DebuggerDisplay => Node.ToJsonString(Options);

    /// <inheritdoc/>
    public override string? ToString()
    {
        return Node.ToJsonString(Options);
    }
}

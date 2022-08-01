using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

[DebuggerDisplay("{Value}")]
public class SystemJsonValue : SystemJsonNode, IJsonValue
{
    public new JsonValue Token => base.Token.AsValue();

    public SystemJsonValue(JsonValue token, JsonSerializerOptions options)
        : base(token, options)
    { }

    // TODO
    public object? Value
    {
        get => Token;
        set => base.Token = JsonValue.Create(value)!;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}

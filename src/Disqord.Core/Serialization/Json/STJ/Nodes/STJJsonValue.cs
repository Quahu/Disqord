using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.STJ.Nodes;

[DebuggerDisplay("{Value}")]
public class STJJsonValue : STJJsonNode, IJsonValue
{
    private readonly JsonSerializerOptions _options;
    public new JsonValue Token => base.Token.AsValue();

    public STJJsonValue(JsonValue token, JsonSerializerOptions options) : base(token)
    {
        _options = options;
    }
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
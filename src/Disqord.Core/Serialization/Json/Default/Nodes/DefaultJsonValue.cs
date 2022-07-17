using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default;

/// <summary>
///     Represents a default JSON value node.
///     Wraps a <see cref="JValue"/>.
/// </summary>
[DebuggerDisplay("{Value}")]
public class DefaultJsonValue : DefaultJsonNode, IJsonValue
{
    /// <inheritdoc cref="DefaultJsonNode.Token"/>
    public new JValue Token => (base.Token as JValue)!;

    /// <inheritdoc/>
    public object? Value
    {
        get => Token.Value;
        set => Token.Value = value;
    }

    public DefaultJsonValue(JValue token, JsonSerializer serializer)
        : base(token, serializer)
    { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Token.ToString(CultureInfo.InvariantCulture);
    }
}
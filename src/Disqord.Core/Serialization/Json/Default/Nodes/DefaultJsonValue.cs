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

    public JsonValueType Type => Token.Type switch
    {
        JTokenType.Object => JsonValueType.Object,
        JTokenType.Array => JsonValueType.Array,
        JTokenType.Integer or JTokenType.Float => JsonValueType.Number,
        JTokenType.String or JTokenType.Date or JTokenType.Raw or JTokenType.Bytes or JTokenType.Guid or JTokenType.Uri or JTokenType.TimeSpan => JsonValueType.String,
        JTokenType.Boolean when Token.Value<bool>() => JsonValueType.True,
        JTokenType.Boolean when !Token.Value<bool>() => JsonValueType.False,
        _ => JsonValueType.Null
    };

    public DefaultJsonValue(JValue token, JsonSerializer serializer)
        : base(token, serializer)
    { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Token.ToString(CultureInfo.InvariantCulture);
    }
}

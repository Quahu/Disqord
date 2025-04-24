using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Newtonsoft;

/// <summary>
///     Represents a default JSON value node.
///     Wraps a <see cref="JValue"/>.
/// </summary>
[DebuggerDisplay("{Value}")]
public class NewtonsoftJsonValue : NewtonsoftJsonNode, IJsonValue
{
    /// <inheritdoc cref="NewtonsoftJsonNode.Token"/>
    public new JValue Token => (base.Token as JValue)!;

    public NewtonsoftJsonValue(JValue token, JsonSerializer serializer)
        : base(token, serializer)
    { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Token.ToString(CultureInfo.InvariantCulture);
    }
}

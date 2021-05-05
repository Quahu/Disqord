using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    /// <summary>
    ///     Represents a default JSON value node.
    ///     Wraps a <see cref="JValue"/>.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class DefaultJsonValue : DefaultJsonNode, IJsonValue
    {
        /// <inheritdoc cref="DefaultJsonNode.Token"/>
        public new JValue Token => base.Token as JValue;

        /// <inheritdoc/>
        public object Value => Token.Value;

        public DefaultJsonValue(JValue token, JsonSerializer serializer)
            : base(token, serializer)
        { }
    }
}

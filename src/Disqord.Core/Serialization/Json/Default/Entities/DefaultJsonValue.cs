using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    [DebuggerDisplay("{Value}")]
    public class DefaultJsonValue : DefaultJsonToken, IJsonValue
    {
        public new JValue Token => base.Token as JValue;

        public object Value => Token.Value;

        public DefaultJsonValue(JValue token, JsonSerializer serializer)
            : base(token, serializer)
        { }

        public override string ToString()
            => Value?.ToString();
    }
}

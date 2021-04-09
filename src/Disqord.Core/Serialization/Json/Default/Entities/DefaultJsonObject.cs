using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    public class DefaultJsonObject : DefaultJsonToken, IJsonObject
    {
        public new JObject Token => base.Token as JObject;

        public IJsonToken this[string key] => Create(Token[key], _serializer);

        public DefaultJsonObject(JObject token, JsonSerializer serializer)
            : base(token, serializer)
        { }
    }
}

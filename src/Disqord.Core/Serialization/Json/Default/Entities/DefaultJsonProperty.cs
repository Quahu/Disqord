using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class DefaultJsonProperty : DefaultJsonToken, IJsonProperty
    {
        public new JProperty Token => base.Token as JProperty;

        public string Name => Token.Name;

        public IJsonToken Value => Create(Token.Value, _serializer);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{Name}: \"{Value}\"";

        public DefaultJsonProperty(JProperty token, JsonSerializer serializer)
            : base(token, serializer)
        { }
    }
}

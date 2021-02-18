using System;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api
{
    public class GatewayDispatchReceivedEventArgs : EventArgs
    {
        public virtual string Name { get; }

        public virtual IJsonToken Data { get; }

        public GatewayDispatchReceivedEventArgs(string name, IJsonToken data)
        {
            Name = name;
            Data = data;
        }
    }
}

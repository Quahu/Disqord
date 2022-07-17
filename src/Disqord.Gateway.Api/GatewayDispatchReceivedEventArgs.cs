using System;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api;

public class GatewayDispatchReceivedEventArgs : EventArgs
{
    public virtual string Name { get; }

    public virtual IJsonNode Data { get; }

    public GatewayDispatchReceivedEventArgs(string name, IJsonNode data)
    {
        Name = name;
        Data = data;
    }
}
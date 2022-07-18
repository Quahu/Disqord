using System;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents gateway dispatch event data.
/// </summary>
public class GatewayDispatchReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the name of the dispatch.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the data of the dispatch.
    /// </summary>
    public IJsonNode Data { get; }

    /// <summary>
    ///     Instantiates a new <see cref="GatewayDispatchReceivedEventArgs"/>.
    /// </summary>
    /// <param name="name"> The name of the dispatch. </param>
    /// <param name="data"> The data of the dispatch. </param>
    public GatewayDispatchReceivedEventArgs(string name, IJsonNode data)
    {
        Name = name;
        Data = data;
    }
}

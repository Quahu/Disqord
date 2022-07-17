using System.Collections.Generic;
using Qommon.Collections.Proxied;

namespace Disqord.Rest.Api;

/// <inheritdoc cref="IRouteParameters"/>
public class RouteParameters : ProxiedDictionary<string, object>, IRouteParameters
{
    /// <summary>
    ///     Instantiates a new <see cref="RouteParameters"/>.
    /// </summary>
    /// <param name="dictionary"> The underlying parameter dictionary. </param>
    public RouteParameters(IDictionary<string, object> dictionary)
        : base(dictionary)
    { }
}

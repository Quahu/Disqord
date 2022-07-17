using System.Collections.Generic;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents the named parameters in the route.
/// </summary>
public interface IRouteParameters : IDictionary<string, object>
{ }

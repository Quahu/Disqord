using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord.Rest.Api
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class RestApiClientExtensions
    {
        private static FormattedRoute Format(Route route, params object[] arguments)
            => route.Format(arguments);

        private static FormattedRoute Format(Route route, IEnumerable<KeyValuePair<string, object>> queryParameters, params object[] arguments)
            => route.Format(arguments, queryParameters);
    }
}

using System;

namespace Disqord.Rest.Api
{
    public sealed class FormattedRoute
    {
        /// <summary>
        ///     Gets the base <see cref="Route"/> this route has been formatted from.
        /// </summary>
        public Route BaseRoute { get; }

        /// <summary>
        ///     Gets the formatted path of the <see cref="BaseRoute"/>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Gets the route parameters found in this route.
        /// </summary>
        public RouteParameters Parameters { get; }

        public FormattedRoute(Route baseRoute, string path, RouteParameters parameters)
        {
            if (baseRoute == null)
                throw new ArgumentNullException(nameof(baseRoute));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            BaseRoute = baseRoute;
            Path = path;
            Parameters = parameters;
        }

        public override string ToString()
            => $"{BaseRoute.Method} | {Path}";
    }
}

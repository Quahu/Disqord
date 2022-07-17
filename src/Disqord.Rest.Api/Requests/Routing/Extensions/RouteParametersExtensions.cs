using System;
using System.ComponentModel;

namespace Disqord.Rest.Api;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class RouteParametersExtensions
{
    /// <summary>
    ///     Gets the major parameter <c>guild_id</c> or <c>0</c>.
    /// </summary>
    public static ulong GetGuildId(this IRouteParameters parameters)
    {
        return parameters.GetValueOrDefault<ulong>("guild_id");
    }

    /// <summary>
    ///     Gets the major parameter <c>channel_id</c> or <c>0</c>.
    /// </summary>
    public static ulong GetChannelId(this IRouteParameters parameters)
    {
        return parameters.GetValueOrDefault<ulong>("channel_id");
    }

    /// <summary>
    ///     Gets the major parameter <c>webhook_id</c> or <c>0</c>.
    /// </summary>
    public static ulong GetWebhookId(this IRouteParameters parameters)
    {
        return parameters.GetValueOrDefault<ulong>("webhook_id");
    }

    /// <summary>
    ///     Retrieves a parameter with the given name and converts it or retrieves the default value for its type.
    /// </summary>
    /// <typeparam name="T"> The type of the parameter. </typeparam>
    /// <param name="parameters"> The route parameters. </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <returns> The value of the parameter or the default value for its type. </returns>
    public static T? GetValueOrDefault<T>(this IRouteParameters parameters, string name)
    {
        if (!parameters.TryGetValue(name, out var value))
            return default;

        if (Convert.GetTypeCode(value) == TypeCode.Object)
            return (T) value;

        return (T) Convert.ChangeType(value, typeof(T));
    }
}

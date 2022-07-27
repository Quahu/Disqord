using System;
using System.ComponentModel;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher;

/// <summary>
///     Represents <see cref="DispatchHandler"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DispatchHandlerExtensions
{
    public static DispatchHandler<TModel, TEventArgs> Intercept<TModel, TEventArgs>(this DispatchHandler<TModel, TEventArgs> dispatchHandler, Action<IShard, TModel> func)
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        return new InterceptingDispatchHandler<TModel, TEventArgs>(dispatchHandler, func);
    }
}

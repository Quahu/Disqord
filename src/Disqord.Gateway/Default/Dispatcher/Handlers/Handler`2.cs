using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public abstract class Handler<TModel, TEventArgs> : Handler<TEventArgs>
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        protected Handler()
        { }

        public override async ValueTask HandleDispatchAsync(IGatewayApiClient shard, IJsonToken data)
        {
            var model = data.ToType<TModel>();
            var eventArgs = await HandleDispatchAsync(shard, model).ConfigureAwait(false);
            if (eventArgs == null || eventArgs == EventArgs.Empty)
                return;

            await InvokeEventAsync(eventArgs).ConfigureAwait(false);
        }

        protected virtual ValueTask InvokeEventAsync(TEventArgs eventArgs)
        {
            // This is the case for most handlers - the dispatch maps to a single event.
            if (Event != null)
            {
                // Invoke READY handlers and wait for them.
                // TODO: RESUME?
                if (eventArgs is ReadyEventArgs)
                    return Event.InvokeAsync(Dispatcher, eventArgs);

                // Don't wait for other events.
                Event.Invoke(Dispatcher, eventArgs);
                return default;
            }

            // The dispatch maps to multiple events. We get the event for the type of the event args.
            if (!_events.TryGetValue(eventArgs.GetType(), out var @event))
                throw new InvalidOperationException($"The dispatch handler {GetType()} returned an invalid instance of event args: {eventArgs.GetType()}.");

            @event.Invoke(Dispatcher, eventArgs);
            return default;
        }

        public abstract ValueTask<TEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model);
    }
}

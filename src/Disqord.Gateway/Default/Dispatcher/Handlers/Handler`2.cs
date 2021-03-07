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

        public override async Task HandleDispatchAsync(IGatewayApiClient shard, IJsonToken data)
        {
            var model = data.ToType<TModel>();
            var task = HandleDispatchAsync(shard, model);
            if (task == null)
                throw new InvalidOperationException($"The dispatch handler {GetType()} returned a null handle task.");

            var eventArgs = await task.ConfigureAwait(false);
            if (eventArgs == null || eventArgs == EventArgs.Empty)
                return;

            await InvokeEventAsync(eventArgs).ConfigureAwait(false);
        }

        public Task InvokeEventAsync(TEventArgs eventArgs)
        {
            if (Event != null)
            {
                // This is the case for most handlers - the dispatch maps to a single event.
                return Event.InvokeAsync(Dispatcher, eventArgs);
            }
            else
            {
                // The dispatch maps to multiple events. We get the event for the type of the event args.
                if (!_events.TryGetValue(eventArgs.GetType(), out var @event))
                    throw new InvalidOperationException($"The dispatch handler {GetType()} returned an invalid instance of event args: {eventArgs.GetType()}.");

                return @event.InvokeAsync(Dispatcher, eventArgs);
            }
        }

        public abstract Task<TEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model);
    }
}

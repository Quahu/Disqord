using System;
using System.Collections.Generic;
using Disqord.Events;

namespace Disqord.Gateway.Default.Dispatcher
{
    public abstract class Handler<TEventArgs> : Handler
        where TEventArgs : EventArgs
    {
        // The pre-fetched event from the dictionary below.
        private protected AsynchronousEvent<TEventArgs> Event { get; private set; }

        // TEventArgs -> AsynchronousEvent<TEventArgs>.
        private protected Dictionary<Type, AsynchronousEvent> _events;

        private protected Handler()
        { }

        public override void Bind(DefaultGatewayDispatcher value)
        {
            base.Bind(value);

            // Gets or adds the map of events for the given dispatcher instance.
            _events = _eventsByDispatcher.GetOrAdd(Dispatcher, dispatcher =>
            {
                var dictionary = new Dictionary<Type, AsynchronousEvent>(_eventsProperties.Length);
                for (var i = 0; i < _eventsProperties.Length; i++)
                {
                    var property = _eventsProperties[i];
                    dictionary.Add(property.PropertyType.GenericTypeArguments[0], (AsynchronousEvent) property.GetValue(dispatcher));
                }

                return dictionary;
            });

            // Some dispatches won't map to events (e.g. READY) or will map to multiple ones (e.g. GUILD_CREATE maps to GuildAvailable and JoinedGuild).
            if (typeof(TEventArgs) == typeof(EventArgs))
                return;

            if (!_events.TryGetValue(typeof(TEventArgs), out var @event))
                throw new ArgumentException($"No {nameof(DefaultGatewayDispatcher)} event found matching the type of {nameof(TEventArgs)}.");

            Event = (AsynchronousEvent<TEventArgs>) @event;
        }
    }
}

using System;
using System.Collections.Generic;
using Qommon.Collections.Synchronized;
using Qommon.Events;

namespace Disqord.Gateway.Default.Dispatcher;

public abstract class DispatchHandler<TEventArgs> : DispatchHandler
    where TEventArgs : EventArgs
{
    // The pre-fetched event from the dictionary below.
    private protected AsynchronousEvent<TEventArgs>? Event { get; private set; }

    // TEventArgs -> AsynchronousEvent<TEventArgs>.
    private protected Dictionary<Type, IAsynchronousEvent>? Events;

    private protected DispatchHandler()
    { }

    public override void Bind(DefaultGatewayDispatcher value)
    {
        base.Bind(value);

        // Gets or adds the map of events for the given dispatcher instance.
        Events = EventsByDispatcher.GetOrAdd(Dispatcher, dispatcher =>
        {
            var dictionary = new Dictionary<Type, IAsynchronousEvent>(EventProperties.Length);
            for (var i = 0; i < EventProperties.Length; i++)
            {
                var property = EventProperties[i];
                dictionary.Add(property.PropertyType.GenericTypeArguments[0], (IAsynchronousEvent) property.GetValue(dispatcher)!);
            }

            return dictionary;
        });

        // Some dispatches won't map to events (e.g. READY) or will map to multiple ones (e.g. GUILD_CREATE maps to GuildAvailable and JoinedGuild).
        if (typeof(TEventArgs) == typeof(EventArgs))
            return;

        if (!Events.TryGetValue(typeof(TEventArgs), out var @event))
            throw new ArgumentException($"No {nameof(DefaultGatewayDispatcher)} event found matching the type of {nameof(TEventArgs)}.");

        Event = (AsynchronousEvent<TEventArgs>) @event;
    }
}

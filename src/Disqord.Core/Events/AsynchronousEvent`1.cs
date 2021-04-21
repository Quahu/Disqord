using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Disqord.Events
{
    /// <summary>
    ///     Represents an asynchronous event handler used by the <see cref="AsynchronousEvent{T}"/>.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type"/> of <see cref="EventArgs"/> used by this handler. </typeparam>
    /// <param name="sender"> The instance from which the event came from. </param>
    /// <param name="e"> The <see cref="EventArgs"/> object containing the event data. </param>
    public delegate ValueTask AsynchronousEventHandler<T>(object sender, T e)
        where T : EventArgs;

    /// <summary>
    ///     Represents an asynchronous event handler caller.
    /// </summary>
    /// <typeparam name="TEventArgs"> The <see cref="Type"/> of <see cref="EventArgs"/> used by this event. </typeparam>
    public sealed class AsynchronousEvent<TEventArgs> : AsynchronousEvent
        where TEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the amount of handlers this <see cref="AsynchronousEvent{T}"/> holds.
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    return _handlers.Count;
                }
            }
        }

        private ImmutableHashSet<AsynchronousEventHandler<TEventArgs>> _handlers;
        private readonly Action<Exception> _exceptionHandler;

        /// <summary>
        ///     Instantiates a new <see cref="AsynchronousEvent{T}"/>.
        /// </summary>
        public AsynchronousEvent()
        {
            _handlers = ImmutableHashSet<AsynchronousEventHandler<TEventArgs>>.Empty;
        }

        /// <summary>
        ///     Instantiates a new <see cref="AsynchronousEvent{T}"/> with the specified <see cref="Func{T, TResult}"/> error handler.
        /// </summary>
        /// <param name="exceptionHandler"> The exception handler for exceptions occurring in event handlers. </param>
        public AsynchronousEvent(Action<Exception> exceptionHandler)
            : this()
        {
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        ///     Hooks an <see cref="AsynchronousEventHandler{T}"/> up to this <see cref="AsynchronousEvent{T}"/>.
        /// </summary>
        /// <param name="handler"> The <see cref="AsynchronousEventHandler{T}"/> to hook up. </param>
        public void Hook(AsynchronousEventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            lock (this)
            {
                var currentHandlers = _handlers;
                var newHandlers = currentHandlers.Add(handler);
                if (currentHandlers == newHandlers)
                    throw new ArgumentException($"The event handler {handler} is already hooked to this event. Did you hook it up twice by mistake?");

                _handlers = newHandlers;
            }
        }

        /// <summary>
        ///     Unhooks an <see cref="AsynchronousEventHandler{T}"/> from this <see cref="AsynchronousEvent{T}"/>.
        /// </summary>
        /// <param name="handler"> The <see cref="AsynchronousEventHandler{T}"/> to unhook. </param>
        public void Unhook(AsynchronousEventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            lock (this)
            {
                var currentHandlers = _handlers;
                var newHandlers = currentHandlers.Remove(handler);
                if (currentHandlers == newHandlers)
                    throw new ArgumentException($"The event handler {handler} was never hooked to this event.");

                _handlers = newHandlers;
            }
        }

        /// <summary>
        ///     Unhooks all <see cref="AsynchronousEventHandler{T}"/>s from this <see cref="AsynchronousEvent{T}"/>.
        /// </summary>
        public void UnhookAll()
        {
            lock (this)
            {
                _handlers = _handlers.Clear();
            }
        }

        protected internal override ValueTask InvokeAsync(object sender, EventArgs e)
            => InvokeAsync(sender, (TEventArgs) e);

        /// <summary>
        ///     Invokes this <see cref="AsynchronousEventHandler{T}"/>, sequentially invoking each hooked up <see cref="AsynchronousEventHandler{T}"/>.
        /// </summary>
        /// <param name="sender"> The sender invoking this event. </param>
        /// <param name="e"> The <see cref="EventArgs"/> data for this invocation. </param>
        public async ValueTask InvokeAsync(object sender, TEventArgs e)
        {
            ImmutableHashSet<AsynchronousEventHandler<TEventArgs>> handlers;
            lock (this)
            {
                handlers = _handlers;
            }

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.Invoke(sender, e).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Invoke(ex);
                }
            }
        }
    }
}

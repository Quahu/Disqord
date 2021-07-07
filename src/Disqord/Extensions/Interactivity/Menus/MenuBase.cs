using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents a menu with a set of message components.
    /// </summary>
    /// <remarks>
    ///     Implementations are free to implement <see cref="IAsyncDisposable"/> as <see cref="InteractivityExtension"/>
    ///     will call both <see cref="IAsyncDisposable.DisposeAsync"/> and <see cref="Dispose"/> for stopping menus.
    /// </remarks>
    public abstract class MenuBase : IDisposable
    {
        /// <summary>
        ///     Gets the extension that started this menu.
        /// </summary>
        /// <remarks>
        ///     This property is lazily set by <see cref="InteractivityExtension.StartMenuAsync(Snowflake, MenuBase, TimeSpan, CancellationToken)"/>,
        ///     thus must not be used prior to it.
        /// </remarks>
        public InteractivityExtension Interactivity { get; internal set; }

        /// <summary>
        ///     Gets the Discord client from the extension.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="Interactivity"/>
        /// </remarks>
        public DiscordClientBase Client => Interactivity.Client;

        /// <summary>
        ///     Gets the channel ID this menu is bound to.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="Interactivity"/>
        /// </remarks>
        public Snowflake ChannelId { get; internal set; }

        /// <summary>
        ///     Gets the message ID this menu is bound to.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="Interactivity"/>
        /// </remarks>
        public Snowflake MessageId { get; internal set; }

        /// <summary>
        ///     Gets the stopping token. The returned token combines cancellation passed via the extension with this menu's timeout.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="Interactivity"/>
        /// </remarks>
        public CancellationToken StoppingToken => _cts.Token;

        /// <summary>
        ///     Gets the <see cref="System.Threading.Tasks.Task"/> that completes when this menu is stopped.
        /// </summary>
        public Task Task => _tcs.Task;

        /// <summary>
        ///     Gets whether this menu has been started and is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        ///     Gets whether this menu has changes.
        /// </summary>
        public bool HasChanges { get; protected set; }

        public ViewBase View
        {
            get => _view;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value.Menu != null && value.Menu != this)
                    throw new ArgumentException("The view belongs to another menu.");

                if (value == _view)
                    return;

                if (_view != null && IsRunning)
                    HasChanges = true;

                value.Menu = this;
                _view = value;
            }
        }
        private ViewBase _view;

        private Cts _cts;
        private TimeSpan _timeout;
        private Timer _timeoutTimer;

        private bool _isDisposed;

        private readonly Tcs _tcs;

        /// <summary>
        ///     Instantiates a new <see cref="MenuBase"/>.
        /// </summary>
        /// <param name="view"> The view for this menu. Can be <see langword="null"/> and set later via <see cref="View"/>. </param>
        protected MenuBase(ViewBase view)
        {
            _tcs = new Tcs();
            View = view;
        }

        /// <summary>
        ///     Refreshes the timeout of this menu.
        ///     By default, is called by <see cref="HandleInteractionAsync"/>.
        /// </summary>
        protected void RefreshTimeout()
            => _timeoutTimer?.Change(_timeout, Timeout.InfiniteTimeSpan);

        /// <summary>
        ///     Throws if this menu is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <summary>
        ///     Throws if this menu's view is not valid.
        /// </summary>
        protected virtual void ValidateView()
        {
            if (_view == null)
                throw new InvalidOperationException("This menu has no view set.");
        }

        /// <summary>
        ///     Initializes this menu and returns the message ID to which it was bound.
        /// </summary>
        /// <returns>
        ///     The message ID this menu was bound to.
        /// </returns>
        protected internal abstract ValueTask<Snowflake> InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Checks if the interaction is valid.
        /// </summary>
        /// <param name="e"> The interaction event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> with a <see cref="bool"/> result specifying whether the interaction is valid or not.
        /// </returns>
        protected virtual ValueTask<bool> CheckInteractionAsync(InteractionReceivedEventArgs e)
            => new(true);

        internal async ValueTask OnInteractionReceived(InteractionReceivedEventArgs e)
        {
            try
            {
                if (!await CheckInteractionAsync(e).ConfigureAwait(false))
                    return;
            }
            catch (Exception ex)
            {
                Interactivity.Logger.LogError(ex, "An exception occurred in the interaction check for menu {0}.", GetType());
                return;
            }

            await HandleInteractionAsync(e).ConfigureAwait(false);
        }

        protected virtual async ValueTask HandleInteractionAsync(InteractionReceivedEventArgs e)
        {
            // When a button is triggered we refresh the menu timeout.
            RefreshTimeout();

            try
            {
                await _view.ExecuteAsync(e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Interactivity.Logger.LogError(ex, "An exception occurred in a component callback for menu {0}.", GetType());
            }
        }

        internal void Start(TimeSpan timeout, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (IsRunning)
                throw new InvalidOperationException("This menu is already running.");

            IsRunning = true;

            static void CancellationCallback(object tuple)
            {
                var (tcs, token) = (ValueTuple<Tcs, CancellationToken>) tuple;
                tcs.Cancel(token);
            }

            _cts = Cts.Linked(Client.StoppingToken, cancellationToken);
            _cts.Token.UnsafeRegister(CancellationCallback, (_tcs, cancellationToken));

            if (timeout != Timeout.InfiniteTimeSpan)
            {
                // We store the timeout so it can be refreshed when a button is triggered in OnButtonAsync.
                _timeout = timeout;
                _timeoutTimer = new Timer(CancellationCallback, (_tcs, _cts.Token), timeout, Timeout.InfiniteTimeSpan);
            }
        }

        /// <summary>
        ///     Stops this menu. Transitions the <see cref="Task"/> to a completed state.
        /// </summary>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the stopping work.
        /// </returns>
        public ValueTask StopAsync()
        {
            ThrowIfDisposed();

            if (!IsRunning)
                return default;

            IsRunning = false;
            _tcs.Complete();
            return default;
        }

        /// <summary>
        ///     Disposes this menu, i.e. disposes the timeout timer and cancellation source.
        /// </summary>
        /// <remarks>
        ///     If overridden by different logic, ensure that the base method is called.
        /// </remarks>
        public virtual void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _timeoutTimer?.Dispose();
            _cts.Dispose();
        }
    }
}

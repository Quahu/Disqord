using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Rest;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents a menu with a set of <see cref="Button"/>s that trigger on reactions.
    /// </summary>
    /// <remarks>
    ///     Implementations are free to implement <see cref="IAsyncDisposable"/> as <see cref="InteractivityExtension"/>
    ///     will call both <see cref="IAsyncDisposable.DisposeAsync"/> and <see cref="Dispose"/> for stopping menus.
    /// </remarks>
    public abstract partial class MenuBase : IDisposable
    {
        /// <summary>
        ///     Gets the extension that started this menu.
        /// </summary>
        /// <remarks>
        ///     This property is lazily set by <see cref="InteractivityExtension.StartMenuAsync(Snowflake, MenuBase, TimeSpan, CancellationToken)"/>,
        ///     thus must not be used prior to it.
        ///     Most attempts of doing so will result in exceptions.
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
        ///     Gets or sets whether this menu manages reactions on the message
        ///     accordingly as buttons get added and removed.
        ///     Defaults to <see langword="true"/>.
        /// </summary>
        public bool ManagesReactions { get; protected set; } = true;

        /// <summary>
        ///     Gets or sets whether this menu's button callbacks trigger whenever
        ///     a user removes a reaction of theirs from the message.
        ///     Defaults to <see langword="true"/>.
        /// </summary>
        public bool TriggersOnRemoval { get; protected set; } = true;

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

        private Cts _cts;
        private TimeSpan _timeout;
        private Timer _timeoutTimer;

        private bool _isDisposed;

        private readonly Tcs _tcs;
        private readonly ISynchronizedDictionary<IEmoji, Button> _buttons;

        /// <summary>
        ///     Instantiates a new <see cref="MenuBase"/>.
        /// </summary>
        protected MenuBase()
        {
            _tcs = new Tcs();
            _buttons = GetButtons(this);
        }

        /// <summary>
        ///     Throws if this menu is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);
        }

        internal async Task OnButtonAsync(ButtonEventArgs e)
        {
            if (!TriggersOnRemoval && !e.WasAdded)
                return;

            if (!_buttons.TryGetValue(e.Emoji, out var button))
                return;

            try
            {
                if (!await CheckReactionAsync(e).ConfigureAwait(false))
                    return;
            }
            catch (Exception ex)
            {
                Interactivity.Logger.LogError(ex, "An exception occurred in a reaction check for menu {0}.", GetType());
                return;
            }

            if (_timeoutTimer != null)
            {
                // When a button is triggered we refresh the menu timeout.
                _timeoutTimer.Change(_timeout, Timeout.InfiniteTimeSpan);
            }

            try
            {
                await button.Callback(e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Interactivity.Logger.LogError(ex, "An exception occurred in a button callback for menu {0}.", GetType());
                return;
            }
        }

        /// <summary>
        ///     Initializes this menu and returns the message ID to which it was bound.
        /// </summary>
        /// <returns>
        ///     The message ID this menu was bound to.
        /// </returns>
        protected internal abstract Task<Snowflake> InitializeAsync();

        /// <summary>
        ///     Checks if the reaction is valid.
        /// </summary>
        /// <param name="e"> The reaction event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> with a <see cref="bool"/> result specifying whether the reaction is valid or not.
        /// </returns>
        protected virtual ValueTask<bool> CheckReactionAsync(ButtonEventArgs e)
             => new(true);

        /// <summary>
        ///     Adds a button to this menu.
        /// </summary>
        /// <remarks>
        ///     If the menu is not running or <see cref="ManagesReactions"/> is <see langword="false"/>,
        ///     the returned <see cref="ValueTask"/> does no asynchronous work, thus is completed.
        /// </remarks>
        /// <param name="button"> The button to add. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the adding work.
        /// </returns>
        public ValueTask AddButtonAsync(Button button)
        {
            ThrowIfDisposed();

            if (button == null)
                throw new ArgumentNullException(nameof(button));

            if (!_buttons.TryAdd(button.Emoji, button))
                throw new ArgumentException("A button for this emoji already exists.", nameof(button));

            if (IsRunning && ManagesReactions)
                return new(Client.AddReactionAsync(ChannelId, MessageId, button.Emoji));

            return default;
        }


        /// <inheritdoc cref="RemoveButtonAsync(IEmoji)"/>
        /// <param name="button"> The button to remove. </param>
        public ValueTask RemoveButtonAsync(Button button)
        {
            ThrowIfDisposed();

            if (button == null)
                throw new ArgumentNullException(nameof(button));

            return RemoveButtonAsync(button.Emoji);
        }

        /// <summary>
        ///     Removes a button from this menu.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="AddButtonAsync(Button)"/>
        /// </remarks>
        /// <param name="emoji"> The emoji of the button to remove. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the removing work.
        /// </returns>
        public ValueTask RemoveButtonAsync(IEmoji emoji)
        {
            ThrowIfDisposed();

            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (!_buttons.TryRemove(emoji, out var button))
                return default;

            if (IsRunning && ManagesReactions)
                return new(Client.RemoveOwnReactionAsync(ChannelId, MessageId, button.Emoji)); // TODO: clear instead?

            return default;
        }

        /// <summary>
        ///     Clears buttons from this menu.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="AddButtonAsync(Button)"/>
        /// </remarks>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the clearing work.
        /// </returns>
        public ValueTask ClearButtonsAsync()
        {
            ThrowIfDisposed();

            _buttons.Clear();
            if (IsRunning && ManagesReactions)
                return new(Client.ClearReactionsAsync(ChannelId, MessageId)); // TODO: permissions

            return default;
        }

        internal async Task StartAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (IsRunning)
                return;

            IsRunning = true;
            if (ManagesReactions)
            {
                var buttons = _buttons.Values;
                Array.Sort(buttons, (a, b) => a.Position.CompareTo(b.Position));
                foreach (var button in buttons)
                    await Client.AddReactionAsync(ChannelId, MessageId, button.Emoji).ConfigureAwait(false);
            }

            static void CancelationCallback(object tuple)
            {
                var (tcs, token) = (ValueTuple<Tcs, CancellationToken>) tuple;
                tcs.Cancel(token);
            }

            _cts = Cts.Linked(Client.StoppingToken, cancellationToken);
            _cts.Token.UnsafeRegister(CancelationCallback, (_tcs, _cts.Token));

            if (timeout != Timeout.InfiniteTimeSpan)
            {
                // We store the timeout so it can be refreshed when a button is triggered in OnButtonAsync.
                _timeout = timeout;
                _timeoutTimer = new Timer(CancelationCallback, (_tcs, _cts.Token), timeout, Timeout.InfiniteTimeSpan);
            }
        }

        /// <summary>
        ///     Stops this menu. Transitions the <see cref="Task"/> to a completed state.
        /// </summary>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the stopping work.
        /// </returns>
        public virtual ValueTask StopAsync()
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
        ///     If overridden by different logic, ensure that this is called.
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

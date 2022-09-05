using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Represents a menu with a set of message components.
/// </summary>
public abstract class MenuBase : IAsyncDisposable
{
    /// <summary>
    ///     Gets the extension that started this menu.
    /// </summary>
    /// <remarks>
    ///     This property is lazily set by <see cref="InteractivityExtension.StartMenuAsync(Snowflake, MenuBase, TimeSpan, CancellationToken)"/>,
    ///     thus must not be used prior to it.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if accessed before the menu is started.
    /// </exception>
    public InteractivityExtension Interactivity
    {
        get
        {
            if (_interactivity == null)
                Throw.InvalidOperationException("This property must not be accessed before the menu is started.");

            return _interactivity;
        }
        internal set => _interactivity = value;
    }
    private InteractivityExtension? _interactivity;

    /// <summary>
    ///     Gets the Discord client from the extension.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="Interactivity"/>
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if accessed before the menu is started.
    /// </exception>
    public DiscordClientBase Client => Interactivity.Client;

    /// <summary>
    ///     Gets the ID of the channel this menu is bound to.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="Interactivity"/>
    /// </remarks>
    public Snowflake ChannelId { get; internal set; }

    /// <summary>
    ///     Gets the ID of the message this menu is bound to.
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
    public CancellationToken StoppingToken => _cts!.Token;

    /// <summary>
    ///     Gets the <see cref="System.Threading.Tasks.Task"/> that completes when this menu is stopped.
    /// </summary>
    public Task Task => _tcs!.Task;

    /// <summary>
    ///     Gets whether this menu has been started and is running.
    /// </summary>
    [MemberNotNullWhen(true, nameof(_tcs))]
    public bool IsRunning
    {
        get
        {
            var tcs = _tcs;
            if (tcs == null)
                return false;

            return !tcs.Task.IsCompleted;
        }
    }

    /// <summary>
    ///     Gets or sets whether this menu has changes.
    /// </summary>
    public bool HasChanges { get; protected set; }

    /// <summary>
    ///     Gets or sets the view of this menu.
    /// </summary>
    /// <exception cref="ArgumentNullException"> The provided value was <see langword="null"/>. </exception>
    /// <exception cref="ArgumentException"> The provided value belonged to another menu. </exception>
    public ViewBase? View
    {
        get => _view;
        set
        {
            Guard.IsNotNull(value);

            if (value._menu != null && value._menu != this)
                throw new ArgumentException("The view belongs to another menu.");

            if (value == _view)
                return;

            if (_view != null && IsRunning)
                HasChanges = true;

            value._menu = this;
            _view = value;
        }
    }
    private ViewBase? _view;

    private Tcs? _tcs;
    private Cts? _cts;
    private TimeSpan _timeout;
    private Timer? _timeoutTimer;

    private bool _isDisposed;
    private readonly object _disposeLock = new();

    /// <summary>
    ///     Instantiates a new <see cref="MenuBase"/>.
    /// </summary>
    /// <param name="view"> The view for this menu. Can be <see langword="null"/> and set later via <see cref="View"/>. </param>
    protected MenuBase(ViewBase view)
    {
        View = view;
    }

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
    ///     Refreshes the timeout of this menu.
    ///     By default, is called by <see cref="HandleInteractionAsync"/>.
    /// </summary>
    protected void RefreshTimeout()
    {
        if (!IsRunning)
            return;

        lock (_disposeLock)
        {
            _timeoutTimer?.Change(_timeout, Timeout.InfiniteTimeSpan);
        }
    }

    /// <summary>
    ///     Initializes this menu and returns the message ID to which it was bound.
    /// </summary>
    /// <returns>
    ///     The ID of the message this menu was bound to.
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
    {
        return new(true);
    }

    /// <summary>
    ///     Handles the given interaction.
    /// </summary>
    /// <remarks>
    ///     By default refreshes the timeout, executes the <see cref="View"/>, and calls <see cref="ApplyChangesAsync"/>.
    /// </remarks>
    /// <param name="e"> The event data. </param>
    protected virtual async ValueTask HandleInteractionAsync(InteractionReceivedEventArgs e)
    {
        var view = _view;
        if (view == null)
            return;

        if (!view.TryExecuteComponent(e, out var task))
            return;

        // If a component is being executed we refresh the menu timeout.
        RefreshTimeout();

        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Interactivity.Logger.LogError(ex, "An exception occurred in a component callback for menu {0}.", GetType());
        }

        if (!IsRunning)
            return;

        try
        {
            await ApplyChangesAsync(e).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Interactivity.Logger.LogError(ex, "An exception occurred while applying menu changes for view {0}.", view.GetType());
        }
    }

    /// <summary>
    ///     Sets <see cref="View"/> to the provided view and disposes of the previous one.
    /// </summary>
    /// <param name="view"> The view to set. </param>
    public virtual async ValueTask SetViewAsync(ViewBase view)
    {
        var oldView = _view;
        if (oldView != null)
        {
            await oldView.DisposeAsync().ConfigureAwait(false);
        }

        View = view;
    }

    /// <summary>
    ///     Creates the <see cref="LocalMessageBase"/> for the view to be formatted into.
    /// </summary>
    /// <returns>
    ///     A new <see cref="LocalMessageBase"/>.
    /// </returns>
    public abstract LocalMessageBase CreateLocalMessage();

    /// <summary>
    ///     Updates the message the menu is bound according to view changes.
    /// </summary>
    /// <param name="e"> The event data. If not provided the message is modified normally, i.e. not by using interaction responses. </param>
    public virtual async ValueTask ApplyChangesAsync(InteractionReceivedEventArgs? e = null)
    {
        var view = View;
        if (view == null)
            return;

        var responseHelper = e?.Interaction.Response();
        if (HasChanges || view.HasChanges)
        {
            // If we have changes, we update the message accordingly.
            await view.UpdateAsync().ConfigureAwait(false);

            var localMessage = CreateLocalMessage();
            view.FormatLocalMessage(localMessage);
            try
            {
                if (responseHelper == null || (responseHelper.HasResponded && responseHelper.ResponseType is not InteractionResponseType.DeferredMessageUpdate))
                {
                    // If there's no interaction provided or the user has already responded (not with DeferredMessageUpdate), modify the message normally.
                    await Client.ModifyMessageAsync(ChannelId, MessageId, x =>
                    {
                        x.Content = localMessage.Content;
                        x.Embeds = Optional.Convert(localMessage.Embeds, embeds => embeds as IEnumerable<LocalEmbed>);
                        x.Components = Optional.Convert(localMessage.Components, components => components as IEnumerable<LocalRowComponent>);
                        x.AllowedMentions = localMessage.AllowedMentions;
                    }).ConfigureAwait(false);
                }
                else
                {
                    if (!responseHelper.HasResponded)
                    {
                        // If the user hasn't responded, respond to the interaction with modifying the message.
                        await responseHelper.ModifyMessageAsync(localMessage is LocalInteractionMessageResponse interactionMessageResponse
                            ? interactionMessageResponse
                            : new LocalInteractionMessageResponse
                            {
                                Content = localMessage.Content,
                                IsTextToSpeech = localMessage.IsTextToSpeech,
                                Embeds = localMessage.Embeds,
                                AllowedMentions = localMessage.AllowedMentions,
                                Components = localMessage.Components
                            }).ConfigureAwait(false);
                    }
                    else
                    {
                        // If the user deferred the response (a button is taking too long, for example), modify the message via a followup.
                        await e!.Interaction.Followup().ModifyResponseAsync(x =>
                        {
                            x.Content = localMessage.Content;
                            x.Embeds = Optional.Convert(localMessage.Embeds, embeds => embeds as IEnumerable<LocalEmbed>);
                            x.Components = Optional.Convert(localMessage.Components, components => components as IEnumerable<LocalRowComponent>);
                            x.AllowedMentions = localMessage.AllowedMentions;
                        }).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                HasChanges = false;
                view.HasChanges = false;
            }
        }
        else if (responseHelper != null && !responseHelper.HasResponded)
        {
            // Acknowledge the interaction to prevent it from failing.
            await responseHelper.DeferAsync().ConfigureAwait(false);
        }
    }

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

        try
        {
            await HandleInteractionAsync(e).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Interactivity.Logger.LogError(ex, "An exception occurred while handling interaction for menu {0}.", GetType());
        }
    }

    internal void Start(TimeSpan timeout, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        if (IsRunning)
            throw new InvalidOperationException("This menu is already running.");

        _tcs = new Tcs();
        _cts = Cts.Linked(Client.StoppingToken, cancellationToken);

        static void CancellationCallback(object? state, CancellationToken cancellationToken)
        {
            var tcs = Unsafe.As<Tcs>(state)!;
            tcs.Cancel(cancellationToken);
        }

        _cts.Token.UnsafeRegister(CancellationCallback, _tcs);

        if (timeout == Timeout.InfiniteTimeSpan)
            return;

        // We store the timeout so it can be refreshed when a button is triggered in HandleInteractionAsync.
        _timeout = timeout;

        static void TimerCallback(object? state)
        {
            var menu = Unsafe.As<MenuBase>(state)!;
            lock (menu._disposeLock)
            {
                if (!menu.IsRunning)
                    return;

                var cts = menu._cts;
                if (cts == null)
                    return;

                cts.Cancel();
                menu._tcs!.Cancel(cts.Token);
            }
        }

        _timeoutTimer = new Timer(TimerCallback, this, timeout, Timeout.InfiniteTimeSpan);
    }

    /// <summary>
    ///     Stops this menu. Transitions the <see cref="Task"/> to a completed state.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if this menu was stopped and <see langword="false"/> if it was already stopped.
    /// </returns>
    public bool Stop()
    {
        ThrowIfDisposed();

        if (!IsRunning)
            return false;

        if (!_tcs.Complete())
            return false;

        _timeoutTimer?.Dispose();
        _timeoutTimer = null;
        return true;
    }

    /// <summary>
    ///     Disposes this menu, i.e. disposes the timeout timer, cancellation source, and view.
    /// </summary>
    /// <remarks>
    ///     This method is called by <see cref="InteractivityExtension"/>.
    ///     If overridden by different logic, ensure that the base method is called.
    /// </remarks>
    public virtual ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return default;

        lock (_disposeLock)
        {
            if (_isDisposed)
                return default;

            _isDisposed = true;
            _cts?.Dispose();
            _cts = null;
            _timeoutTimer?.Dispose();
            _timeoutTimer = null;
        }

        var view = _view;
        if (view != null)
        {
            return view.DisposeAsync();
        }

        return default;
    }
}

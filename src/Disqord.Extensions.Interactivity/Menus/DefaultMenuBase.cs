using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Represents the default menu, i.e. a menu that sends a message
///     on initialization and is limited to a specific author.
/// </summary>
public abstract class DefaultMenuBase : MenuBase
{
    /// <summary>
    ///     Gets or sets the message of this menu.
    ///     Returns <see langword="null"/> if the menu was created with a pre-existing message ID.
    /// </summary>
    /// <remarks>
    ///     This property exists purely not to lose data from the sent message in <see cref="InitializeAsync"/>.
    ///     <see cref="MenuBase.MessageId"/> should be preferred for all operations.
    /// </remarks>
    public IUserMessage? Message { get; protected set; }

    /// <summary>
    ///     Gets or sets the ID of the user this menu is restricted to.
    ///     Defaults to <see langword="null"/>.
    ///     If <see langword="null"/> all users can interact with the menu.
    /// </summary>
    public Snowflake? AuthorId { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultMenuBase"/>.
    /// </summary>
    /// <param name="view"> The initial view. </param>
    protected DefaultMenuBase(ViewBase view)
        : base(view)
    { }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultMenuBase"/> with a pre-existing message ID.
    ///     This makes <see cref="InitializeAsync"/> return it instead of sending a new message.
    /// </summary>
    /// <param name="view"> The initial view. </param>
    /// <param name="messageId"> The ID of the message to use for this menu. If one is provided, <see cref="InitializeAsync"/> does not send a new message. </param>
    /// <remarks>
    ///     This does <b>not</b> persist the view's state as it has no way of doing so.
    ///     E.g. if you were to use a <see cref="PagedViewBase"/>, the <see cref="PagedViewBase.CurrentPageIndex"/> must be restored manually.
    /// </remarks>
    protected DefaultMenuBase(ViewBase view, Snowflake messageId)
        : this(view)
    {
        MessageId = messageId;
    }

    /// <summary>
    ///     Initializes this menu by sending a message created from the view.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the initialization work
    ///     with the result being the ID of the sent message.
    /// </returns>
    protected internal override async ValueTask<Snowflake> InitializeAsync(CancellationToken cancellationToken)
    {
        ValidateView();
        await View!.UpdateAsync().ConfigureAwait(false);

        var messageId = MessageId;
        if (messageId != default)
            return messageId;

        var message = CreateLocalMessage();
        View.FormatLocalMessage(message);
        Message = await SendLocalMessageAsync(message, cancellationToken).ConfigureAwait(false);
        return Message.Id;
    }

    /// <summary>
    ///     Sends the view message.
    /// </summary>
    /// <param name="message"> The view message. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the work with the result being an <see cref="IUserMessage"/>.
    /// </returns>
    protected abstract Task<IUserMessage> SendLocalMessageAsync(LocalMessageBase message, CancellationToken cancellationToken);

    /// <inheritdoc/>
    /// <summary>
    ///     Checks if the ID of the user who reacted is equal to <see cref="AuthorId"/>.
    /// </summary>
    protected override ValueTask<bool> CheckInteractionAsync(InteractionReceivedEventArgs e)
    {
        var authorId = AuthorId;
        if (authorId == null)
            return new(true);

        return new(e.AuthorId == authorId);
    }
}
using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus.Prompt;

/// <summary>
///     Represents a view displaying "confirm" and "deny" buttons.
/// </summary>
public class PromptView : ViewBase
{
    /// <summary>
    ///     Gets or sets the result of the prompt view.
    /// </summary>
    public bool Result { get; protected set; }

    /// <summary>
    ///     Gets the "confirm" button.
    /// </summary>
    public ButtonViewComponent ConfirmButton { get; }

    /// <summary>
    ///     Gets the "deny" button.
    /// </summary>
    public ButtonViewComponent DenyButton { get; }

    /// <summary>
    ///     Gets or sets the message sent when the "deny" button is clicked.
    /// </summary>
    public string? AbortMessage { get; set; } = "Action aborted.";

    /// <summary>
    ///     Instantiates a new <see cref="PromptView"/> with the specified message template.
    /// </summary>
    /// <param name="messageTemplate"> The message template for this view. </param>
    public PromptView(Action<LocalMessageBase> messageTemplate)
        : base(messageTemplate)
    {
        ConfirmButton = new ButtonViewComponent(OnConfirmButton)
        {
            Label = "Confirm",
            Style = LocalButtonComponentStyle.Success
        };

        DenyButton = new ButtonViewComponent(OnDenyButton)
        {
            Label = "Deny",
            Style = LocalButtonComponentStyle.Danger
        };

        AddComponent(ConfirmButton);
        AddComponent(DenyButton);
    }

    /// <summary>
    ///     Called when the <see cref="ConfirmButton"/> is triggered.
    /// </summary>
    /// <param name="e"> The event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    protected virtual ValueTask OnConfirmButton(ButtonEventArgs e)
    {
        return CompleteAsync(true, e);
    }

    /// <summary>
    ///     Called when the <see cref="DenyButton"/> is triggered.
    /// </summary>
    /// <param name="e"> The event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    protected virtual ValueTask OnDenyButton(ButtonEventArgs e)
    {
        return CompleteAsync(false, e);
    }

    /// <summary>
    ///     Called by <see cref="OnConfirmButton"/> and <see cref="OnDenyButton"/>.
    /// </summary>
    /// <param name="result"> <see langword="true"/> for <see cref="OnConfirmButton"/> and <see langword="false"/> for <see cref="OnDenyButton"/>. </param>
    /// <param name="e"> The event data. </param>
    protected virtual async ValueTask CompleteAsync(bool result, ButtonEventArgs e)
    {
        Result = result;
        try
        {
            var task = result || AbortMessage == null
                ? Menu.Client.DeleteMessageAsync(Menu.ChannelId, Menu.MessageId)
                : e.Interaction.Response().ModifyMessageAsync(new LocalInteractionMessageResponse().WithContent(AbortMessage));

            await task.ConfigureAwait(false);
        }
        finally
        {
            Menu.Stop();
        }
    }
}

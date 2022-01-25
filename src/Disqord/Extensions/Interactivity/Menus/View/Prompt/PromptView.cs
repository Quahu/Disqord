using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus.Prompt
{
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

        public PromptView(LocalMessage message)
            : base(message)
        {
            ConfirmButton = new ButtonViewComponent(OnConfirmButtonAsync)
            {
                Label = "Confirm",
                Style = LocalButtonComponentStyle.Success
            };

            DenyButton = new ButtonViewComponent(OnDenyButtonAsync)
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
        protected virtual ValueTask OnConfirmButtonAsync(ButtonEventArgs e)
            => CompleteAsync(true, e);

        /// <summary>
        ///     Called when the <see cref="DenyButton"/> is triggered.
        /// </summary>
        /// <param name="e"> The event data. </param>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the callback work.
        /// </returns>
        protected virtual ValueTask OnDenyButtonAsync(ButtonEventArgs e)
            => CompleteAsync(false, e);

        protected virtual async ValueTask CompleteAsync(bool result, ButtonEventArgs e)
        {
            Result = result;
            try
            {
                var task = result
                    ? Menu.Client.DeleteMessageAsync(Menu.ChannelId, Menu.MessageId)
                    : e.Interaction.Response().ModifyMessageAsync(new LocalInteractionMessageResponse().WithContent("Action aborted."));

                await task.ConfigureAwait(false);
            }
            finally
            {
                Menu.Stop();
            }
        }
    }
}

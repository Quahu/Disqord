using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;

namespace Disqord.Extensions.Interactivity.Menus
{
    public class InteractiveMenu : AuthorMenuBase
    {
        /// <summary>
        ///     Gets the message of this menu.
        /// </summary>
        public IUserMessage Message { get; protected set; }

        /// <inheritdoc/>
        public InteractiveMenu(Snowflake authorId, ViewBase view)
            : base(authorId, view)
        { }

        /// <summary>
        ///     Initializes this menu by sending a message created from the view.
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to observe. </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the initialization work.
        /// </returns>
        protected internal override async ValueTask<Snowflake> InitializeAsync(CancellationToken cancellationToken)
        {
            ValidateView();

            await View.UpdateAsync().ConfigureAwait(false);

            Message = await Client.SendMessageAsync(ChannelId, View.ToLocalMessage(), new DefaultRestRequestOptions
            {
                CancellationToken = cancellationToken
            });
            return Message.Id;
        }

        protected override async ValueTask HandleInteractionAsync(InteractionReceivedEventArgs e)
        {
            await base.HandleInteractionAsync(e).ConfigureAwait(false);

            if (!IsRunning)
                return;

            try
            {
                await ApplyChangesAsync(e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Interactivity.Logger.LogError(ex, "An exception occurred while applying menu changes for view {0}.", View.GetType());
            }
        }

        public virtual async ValueTask ApplyChangesAsync(InteractionReceivedEventArgs e = null)
        {
            var view = View;
            if (view == null)
                return;

            // If we have changes, we update the message accordingly.
            var response = e?.Interaction.Response();
            if (HasChanges || view.HasChanges)
            {
                await view.UpdateAsync().ConfigureAwait(false);

                var localMessage = view.ToLocalMessage();
                try
                {
                    if (response != null && !response.HasResponded)
                    {
                        // If the user hasn't responded, respond to the interaction with modifying the message.
                        await response.ModifyMessageAsync(new LocalInteractionResponse
                        {
                            Content = localMessage.Content,
                            IsTextToSpeech = localMessage.IsTextToSpeech,
                            Embeds = localMessage.Embeds,
                            AllowedMentions = localMessage.AllowedMentions,
                            Components = localMessage.Components
                        }).ConfigureAwait(false);
                    }
                    else if (response != null && response.HasResponded && response.ResponseType is InteractionResponseType.DeferredMessageUpdate)
                    {
                        // If the user deferred the response (a button is taking too long, for example), modify the message via a followup.
                        await e.Interaction.Followup().ModifyResponseAsync(x =>
                        {
                            x.Content = localMessage.Content;
                            x.Embeds = new Optional<IEnumerable<LocalEmbed>>(localMessage.Embeds);
                            x.Components = new Optional<IEnumerable<LocalRowComponent>>(localMessage.Components);
                            x.AllowedMentions = localMessage.AllowedMentions;
                        }).ConfigureAwait(false);
                    }
                    else
                    {
                        // If the user has responded, modify the message normally.
                        await Message.ModifyAsync(x =>
                        {
                            x.Content = localMessage.Content;
                            x.Embeds = new Optional<IEnumerable<LocalEmbed>>(localMessage.Embeds);
                            x.Components = new Optional<IEnumerable<LocalRowComponent>>(localMessage.Components);
                            x.AllowedMentions = localMessage.AllowedMentions;
                        }).ConfigureAwait(false);
                    }
                }
                finally
                {
                    HasChanges = false;
                    view.HasChanges = false;
                }
            }
            else if (response != null && !response.HasResponded)
            {
                // Acknowledge the interaction to prevent the interaction from failing.
                await response.DeferAsync().ConfigureAwait(false);
            }
        }
    }
}

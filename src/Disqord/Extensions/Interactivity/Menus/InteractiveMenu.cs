using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;

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

            await ApplyChangesAsync(e).ConfigureAwait(false);
        }

        public virtual async ValueTask ApplyChangesAsync(InteractionReceivedEventArgs e = null)
        {
            if (!IsRunning)
                return;

            // If we have changes, we update the message accordingly.
            var response = e?.Interaction.Response();
            if (HasChanges || View.HasChanges)
            {
                await View.UpdateAsync().ConfigureAwait(false);

                var localMessage = View.ToLocalMessage();
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
                        });
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
                    View.HasChanges = false;
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

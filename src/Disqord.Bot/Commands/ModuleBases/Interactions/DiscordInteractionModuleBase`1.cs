using System;
using System.Collections.Generic;
using Disqord.Extensions.Interactivity.Menus;
using Qommon;

namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents a module base for interaction commands.
/// </summary>
/// <typeparam name="TContext"> The command context type. </typeparam>
public abstract class DiscordInteractionModuleBase<TContext> : DiscordModuleBase<TContext>
    where TContext : IDiscordInteractionCommandContext
{
    private protected DiscordInteractionModuleBase()
    { }

    /// <summary>
    ///     Returns a result that will defer the interaction response.
    /// </summary>
    /// <param name="isEphemeral"> Whether the followup should be ephemeral. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    public virtual DiscordInteractionDeferralCommandResult Deferral(bool isEphemeral = false)
        => new(Context, isEphemeral);

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified content.
    /// </summary>
    /// <param name="content"> The content to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionResponseCommandResult Response(string content)
        => Response(new LocalInteractionMessageResponse().WithContent(content));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified embeds.
    /// </summary>
    /// <param name="embeds"> The embeds to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionResponseCommandResult Response(params LocalEmbed[] embeds)
        => Response(new LocalInteractionMessageResponse().WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified content and embeds.
    /// </summary>
    /// <param name="content"> The content to respond with. </param>
    /// <param name="embeds"> The embeds to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionResponseCommandResult Response(string content, params LocalEmbed[] embeds)
        => Response(new LocalInteractionMessageResponse().WithContent(content).WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified message components.
    /// </summary>
    /// <param name="components"> The message components to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionResponseCommandResult Response(params LocalComponent[] components)
        => Response(new LocalInteractionMessageResponse().WithComponents(components));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified message.
    /// </summary>
    /// <param name="message"> The message to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionResponseCommandResult Response(LocalInteractionMessageResponse message)
    {
        if (message.AllowedMentions.GetValueOrDefault() == null)
            message.AllowedMentions = LocalAllowedMentions.None;

        return new(Context, message);
    }

    /// <summary>
    ///     Returns a result that will respond to the interaction with a modal
    ///     containing the specified components.
    /// </summary>
    /// <param name="customId"> The custom ID of the modal. </param>
    /// <param name="title"> The title of the modal. </param>
    /// <param name="components"> The components of the modal. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionModalCommandResult Modal(string customId, string title, params LocalComponent[] components)
    {
        return Modal(new LocalInteractionModalResponse
        {
            CustomId = customId,
            Title = title,
            Components = new List<LocalComponent>(components)
        });
    }

    /// <summary>
    ///     Returns a result that will respond to the interaction with the specified modal.
    /// </summary>
    /// <param name="modal"> The modal to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordInteractionModalCommandResult Modal(LocalInteractionModalResponse modal)
    {
        return new(Context, modal);
    }

    /// <inheritdoc/>
    protected override DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default)
    {
        return Menu(new DefaultInteractionMenu(view, Context.Interaction)
        {
            AuthorId = Context.Author.Id
        }, timeout);
    }
}

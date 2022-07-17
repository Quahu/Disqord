using System;
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

    /// <inheritdoc/>
    protected override DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default)
    {
        return Menu(new DefaultInteractionMenu(view, Context.Interaction)
        {
            AuthorId = Context.Author.Id
        }, timeout);
    }
}

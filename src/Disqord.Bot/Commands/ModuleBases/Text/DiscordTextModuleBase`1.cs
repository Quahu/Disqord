using System;
using Disqord.Extensions.Interactivity.Menus;
using Qommon;

namespace Disqord.Bot.Commands.Text;

/// <summary>
///     Represents a module base for text commands.
/// </summary>
/// <typeparam name="TContext"> The command context type. </typeparam>
public abstract class DiscordTextModuleBase<TContext> : DiscordModuleBase<TContext>
    where TContext : IDiscordTextCommandContext
{
    private protected DiscordTextModuleBase()
    { }

    /// <summary>
    ///     Returns a result that will reply to the context message with the specified content.
    /// </summary>
    /// <param name="content"> The content to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Reply(string content)
        => Reply(new LocalMessage().WithContent(content));

    /// <summary>
    ///     Returns a result that will reply to the context message with the specified embeds.
    /// </summary>
    /// <param name="embeds"> The embeds to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Reply(params LocalEmbed[] embeds)
        => Reply(new LocalMessage().WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will reply to the context message with the specified content and embeds.
    /// </summary>
    /// <param name="content"> The content to reply with. </param>
    /// <param name="embeds"> The embeds to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Reply(string content, params LocalEmbed[] embeds)
        => Reply(new LocalMessage().WithContent(content).WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will reply to the context message with the specified message.
    /// </summary>
    /// <param name="message"> The message to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Reply(LocalMessage message)
        => Response(message.WithReply(Context.Message.Id, Context.ChannelId, Context.GuildId));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified content.
    /// </summary>
    /// <param name="content"> The content to respond with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Response(string content)
        => Response(new LocalMessage().WithContent(content));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified embeds.
    /// </summary>
    /// <param name="embeds"> The embeds to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Response(params LocalEmbed[] embeds)
        => Response(new LocalMessage().WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified content and embeds.
    /// </summary>
    /// <param name="content"> The content to reply with. </param>
    /// <param name="embeds"> The embeds to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Response(string content, params LocalEmbed[] embeds)
        => Response(new LocalMessage().WithContent(content).WithEmbeds(embeds));

    /// <summary>
    ///     Returns a result that will respond in the context channel with the specified message.
    /// </summary>
    /// <param name="message"> The message to reply with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordTextResponseCommandResult Response(LocalMessage message)
    {
        if (message.AllowedMentions.GetValueOrDefault() == null)
            message.AllowedMentions = LocalAllowedMentions.None;

        return new(Context, message);
    }

    /// <summary>
    ///     Returns a result that will react to the context message with the specified emoji.
    /// </summary>
    /// <param name="emoji"> The emoji to react with. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordReactionCommandResult Reaction(LocalEmoji emoji)
        => new(Context, emoji);

    /// <inheritdoc/>
    protected override DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default)
    {
        return Menu(new DefaultTextMenu(view)
        {
            AuthorId = Context.Author.Id
        }, timeout);
    }
}

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity;

/// <summary>
///     Represents <see cref="IDiscordCommandContext"/> interactivity extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class InteractivityCommandContextExtensions
{
    /// <summary>
    ///     Waits for an interaction of type <typeparamref name="TInteraction"/>
    ///     in the context channel from the context author.
    /// </summary>
    /// <param name="context"> The command context instance. </param>
    /// <param name="predicate"> The predicate to filter the messages with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <typeparam name="TInteraction"> The type of the interaction to wait for. </typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching <typeparamref name="TInteraction"/>
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this IDiscordCommandContext context,
        Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction
    {
        predicate = (interaction => interaction.Author.Id == context.AuthorId) + predicate;
        var extension = context.Bot.GetInteractivity();
        return extension.WaitForInteractionAsync(context.ChannelId, predicate, timeout, cancellationToken);
    }

    /// <summary>
    ///     Waits for an interaction of type <typeparamref name="TInteraction"/>
    ///     in the context channel from the context author with the specified custom ID.
    /// </summary>
    /// <param name="context"> The command context instance. </param>
    /// <param name="customId"> The custom ID of the interaction to wait for. </param>
    /// <param name="predicate"> The predicate to filter the messages with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <typeparam name="TInteraction"> The type of the interaction to wait for. </typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching <typeparamref name="TInteraction"/>
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this IDiscordCommandContext context,
        string customId, Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction, ICustomIdentifiableEntity
    {
        predicate = (interaction => interaction.Author.Id == context.AuthorId) + predicate;
        var extension = context.Bot.GetInteractivity();
        return extension.WaitForInteractionAsync(context.ChannelId, customId, predicate, timeout, cancellationToken);
    }

    /// <summary>
    ///     Waits for an interaction in the context channel from the context author.
    /// </summary>
    /// <param name="context"> The command context instance. </param>
    /// <param name="predicate"> The predicate to filter the messages with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching interaction event data
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<InteractionReceivedEventArgs?> WaitForInteractionAsync(this IDiscordCommandContext context,
        Predicate<InteractionReceivedEventArgs>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        predicate = (e => e.AuthorId == context.AuthorId) + predicate;
        var extension = context.Bot.GetInteractivity();
        return extension.WaitForInteractionAsync(context.ChannelId, predicate, timeout, cancellationToken);
    }

    /// <summary>
    ///     Waits for a message in the context channel from the context author.
    /// </summary>
    /// <param name="context"> The command context instance. </param>
    /// <param name="predicate"> The predicate to filter the messages with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching message event data
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<MessageReceivedEventArgs?> WaitForMessageAsync(this IDiscordCommandContext context,
        Predicate<MessageReceivedEventArgs>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        predicate = (e => e.Message.Author.Id == context.AuthorId) + predicate;
        var extension = context.Bot.GetInteractivity();
        return extension.WaitForMessageAsync(context.ChannelId, predicate, timeout, cancellationToken);
    }
}

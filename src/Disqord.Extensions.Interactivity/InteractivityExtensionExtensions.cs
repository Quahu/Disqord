using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class InteractivityExtensionExtensions
{
    /// <summary>
    ///     Waits for an interaction of type <typeparamref name="TInteraction"/>
    ///     in the channel with the specified <paramref name="channelId"/>.
    /// </summary>
    /// <param name="interactivity"> The interactivity extension. </param>
    /// <param name="channelId"> The ID of the channel to wait for the interaction in. </param>
    /// <param name="predicate"> The predicate to filter the interactions with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching <typeparamref name="TInteraction"/>
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this InteractivityExtension interactivity,
        Snowflake channelId, Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction
    {
        var predicateList = Unsafe.As<Predicate<TInteraction>[]>(predicate?.GetInvocationList());

        bool InteractionPredicate(InteractionReceivedEventArgs e)
        {
            if (e.Interaction is not TInteraction interaction)
                return false;

            var predicates = predicateList;
            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    if (!predicate(interaction))
                        return false;
                }
            }

            return true;
        }

        return interactivity.WaitForInteractionAsync(channelId, InteractionPredicate, timeout, cancellationToken)
            .ContinueWith(task => task.Result?.Interaction as TInteraction, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
    }

    /// <summary>
    ///     Waits for an interaction of type <typeparamref name="TInteraction"/>
    ///     in the channel with the specified <paramref name="channelId"/>.
    /// </summary>
    /// <param name="interactivity"> The interactivity extension. </param>
    /// <param name="channelId"> The ID of the channel to wait for the interaction in. </param>
    /// <param name="customId"> The custom ID of the interaction to wait for. </param>
    /// <param name="predicate"> The predicate to filter the interactions with. </param>
    /// <param name="timeout"> The timeout of the wait. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the wait with the result being the matching <typeparamref name="TInteraction"/>
    ///     or <see langword="null"/> if the wait timed out.
    /// </returns>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this InteractivityExtension interactivity,
        Snowflake channelId, string customId, Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction, ICustomIdentifiableEntity
    {
        predicate = (interaction => interaction.CustomId == customId) + predicate;
        return interactivity.WaitForInteractionAsync(channelId, predicate, timeout, cancellationToken);
    }
}

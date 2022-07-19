using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class InteractivityClientExtensions
{
    /// <summary>
    ///     Gets the <see cref="InteractivityExtension"/> instance from this client instance.
    /// </summary>
    /// <param name="client"> The client instance. </param>
    /// <returns>
    ///     The <see cref="InteractivityExtension"/> instance.
    /// </returns>
    public static InteractivityExtension GetInteractivity(this DiscordClientBase client)
    {
        return client.GetRequiredExtension<InteractivityExtension>();
    }

    /// <inheritdoc cref="InteractivityExtensionExtensions.WaitForInteractionAsync{TInteraction}(Disqord.Extensions.Interactivity.InteractivityExtension,Disqord.Snowflake,System.Predicate{TInteraction},System.TimeSpan,System.Threading.CancellationToken)"/>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this DiscordClientBase client,
        Snowflake channelId, Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction
    {
        var extension = client.GetInteractivity();
        return extension.WaitForInteractionAsync(channelId, predicate, timeout, cancellationToken);
    }

    /// <inheritdoc cref="InteractivityExtensionExtensions.WaitForInteractionAsync{TInteraction}(Disqord.Extensions.Interactivity.InteractivityExtension,Disqord.Snowflake,System.String,System.Predicate{TInteraction},System.TimeSpan,System.Threading.CancellationToken)"/>
    public static Task<TInteraction?> WaitForInteractionAsync<TInteraction>(this DiscordClientBase client,
        Snowflake channelId, string customId, Predicate<TInteraction>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
        where TInteraction : class, IUserInteraction, ICustomIdentifiableEntity
    {
        var extension = client.GetInteractivity();
        return extension.WaitForInteractionAsync(channelId, customId, predicate, timeout, cancellationToken);
    }

    /// <inheritdoc cref="InteractivityExtension.WaitForInteractionAsync"/>
    public static Task<InteractionReceivedEventArgs?> WaitForInteractionAsync(this DiscordClientBase client,
        Snowflake channelId, Predicate<InteractionReceivedEventArgs>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        var extension = client.GetInteractivity();
        return extension.WaitForInteractionAsync(channelId, predicate, timeout, cancellationToken);
    }

    /// <inheritdoc cref="InteractivityExtension.WaitForMessageAsync"/>
    public static Task<MessageReceivedEventArgs?> WaitForMessageAsync(this DiscordClientBase client,
        Snowflake channelId, Predicate<MessageReceivedEventArgs>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        var extension = client.GetInteractivity();
        return extension.WaitForMessageAsync(channelId, predicate, timeout, cancellationToken);
    }

    /// <inheritdoc cref="InteractivityExtension.WaitForReactionAsync"/>
    public static Task<ReactionAddedEventArgs?> WaitForReactionAsync(this DiscordClientBase client,
        Snowflake messageId, Predicate<ReactionAddedEventArgs>? predicate = null,
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        var extension = client.GetInteractivity();
        return extension.WaitForReactionAsync(messageId, predicate, timeout, cancellationToken);
    }
}

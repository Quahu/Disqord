using System;
using System.ComponentModel;
using Qommon;

namespace Disqord;

/// <summary>
///     Defines <see cref="IInteraction"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class InteractionExtensions
{
    /// <summary>
    ///     Gets the time elapsed since this interaction was received.
    /// </summary>
    /// <returns>
    ///     A <see cref="TimeSpan"/> representing the elapsed time.
    /// </returns>
    public static TimeSpan GetElapsedTime(this IInteraction interaction)
    {
        Guard.IsNotNull(interaction);

        return StopwatchUtilities.GetElapsedTime(interaction.__ReceivedAt);
    }

    /// <summary>
    ///     Gets whether this interaction is response expired,
    ///     i.e. whether the <see cref="Discord.Limits.Interaction.ResponseTimeout"/> has elapsed.
    /// </summary>
    /// <param name="interaction"> The interaction to check for expiry. </param>
    /// <returns>
    ///     <see langword="true"/> if the interaction is response expired.
    /// </returns>
    public static bool IsResponseExpired(this IInteraction interaction)
    {
        return interaction.GetElapsedTime() >= Discord.Limits.Interaction.ResponseTimeout;
    }

    /// <summary>
    ///     Gets whether this interaction is expired,
    ///     i.e. whether the <see cref="Discord.Limits.Interaction.FollowupTimeout"/> has elapsed.
    /// </summary>
    /// <param name="interaction"> The interaction to check for expiry. </param>
    /// <returns>
    ///     <see langword="true"/> if the interaction is expired.
    /// </returns>
    public static bool IsExpired(this IInteraction interaction)
    {
        return interaction.GetElapsedTime() >= Discord.Limits.Interaction.FollowupTimeout;
    }
}

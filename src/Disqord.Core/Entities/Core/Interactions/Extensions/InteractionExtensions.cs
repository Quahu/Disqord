using System;
using System.ComponentModel;
using System.Diagnostics;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Defines <see cref="IInteraction"/> extensions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractionExtensions
    {
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private static readonly double TickFrequency = (double) TicksPerSecond / Stopwatch.Frequency;

        /// <summary>
        ///     Gets the time elapsed since this interaction was received.
        /// </summary>
        /// <returns>
        ///     A <see cref="TimeSpan"/> representing the elapsed time.
        /// </returns>
        public static TimeSpan GetElapsedTime(this IInteraction interaction)
        {
            Guard.IsNotNull(interaction);

            // TODO: update to Stopwatch.GetElapsedTime() with .NET 7
            return GetElapsedTime(interaction.__ReceivedAt);
        }

        /// <summary>
        ///     Gets whether this interaction is response expired,
        ///     i.e. whether the <see cref="Discord.Limits.Interactions.ResponseTimeout"/> has elapsed.
        /// </summary>
        /// <param name="interaction"> The interaction to check for expiry. </param>
        /// <returns>
        ///     <see langword="true"/> if the interaction is response expired.
        /// </returns>
        public static bool IsResponseExpired(this IInteraction interaction)
        {
            return interaction.GetElapsedTime() >= Discord.Limits.Interactions.ResponseTimeout;
        }

        /// <summary>
        ///     Gets whether this interaction is expired,
        ///     i.e. whether the <see cref="Discord.Limits.Interactions.FollowupTimeout"/> has elapsed.
        /// </summary>
        /// <param name="interaction"> The interaction to check for expiry. </param>
        /// <returns>
        ///     <see langword="true"/> if the interaction is expired.
        /// </returns>
        public static bool IsExpired(this IInteraction interaction)
        {
            return interaction.GetElapsedTime() >= Discord.Limits.Interactions.FollowupTimeout;
        }

        private static TimeSpan GetElapsedTime(long timestamp)
        {
            return TimeSpan.FromTicks((long) ((Stopwatch.GetTimestamp() - timestamp) * TickFrequency));
        }
    }
}

using System;

namespace Disqord.Extensions.Interactivity
{
    public class InteractivityExtensionConfiguration
    {
        /// <summary>
        ///     Gets or sets the default timeout used for awaiting messages.
        ///     Defaults to <c>15</c> seconds.
        /// </summary>
        public virtual TimeSpan DefaultMessageTimeout { get; set; } = TimeSpan.FromSeconds(15);

        /// <summary>
        ///     Gets or sets the default timeout used for awaiting reactions.
        ///     Defaults to <c>15</c> seconds.
        /// </summary>
        public virtual TimeSpan DefaultReactionTimeout { get; set; } = TimeSpan.FromSeconds(15);
    }
}
using System;

namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for interactions.
        /// </summary>
        public static class Interaction
        {
            /// <summary>
            ///     Gets the time in which the interaction can to be responded to.
            /// </summary>
            /// <remarks>
            ///     The value is <c>3</c> seconds.
            /// </remarks>
            public static TimeSpan ResponseTimeout => TimeSpan.FromSeconds(3);

            /// <summary>
            ///     Gets the time in which the interaction can be followed up on.
            /// </summary>
            /// <remarks>
            ///     The value is <c>15</c> minutes.
            /// </remarks>
            public static TimeSpan FollowupTimeout => TimeSpan.FromMinutes(15);

            /// <summary>
            ///     Represents limits for interaction modals.
            /// </summary>
            public static class Modal
            {
                /// <summary>
                ///     The maximum length of custom IDs.
                /// </summary>
                public const int MaxCustomIdLength = 100;

                /// <summary>
                ///     The maximum length of titles.
                /// </summary>
                public const int MaxTitleLength = 45;
            }
        }
    }
}

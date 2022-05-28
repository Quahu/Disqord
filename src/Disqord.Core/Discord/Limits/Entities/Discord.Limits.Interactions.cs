using System;

namespace Disqord
{
    public static partial class Discord
    {
        public static partial class Limits
        {
            public static class Interactions
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

                public static class Modals
                {
                    public const int MaxCustomIdLength = 100;

                    public const int MaxTitleLength = 45;
                }
            }
        }
    }
}

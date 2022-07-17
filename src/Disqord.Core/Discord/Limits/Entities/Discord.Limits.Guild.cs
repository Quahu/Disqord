namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for guilds.
        /// </summary>
        public static class Guild
        {
            /// <summary>
            ///     Represents limits for guild events.
            /// </summary>
            public static class Event
            {
                /// <summary>
                ///     The minimum length of the name.
                /// </summary>
                public const int MinNameLength = 1;

                /// <summary>
                ///     The maximum length of the name.
                /// </summary>
                public const int MaxNameLength = 100;

                /// <summary>
                ///     The minimum length of the description.
                /// </summary>
                public const int MinDescriptionLength = 1;

                /// <summary>
                ///     The maximum length of the description.
                /// </summary>
                public const int MaxDescriptionLength = 1000;

                /// <summary>
                ///     Represents limits for guild event metadata.
                /// </summary>
                public static class Metadata
                {
                    /// <summary>
                    ///     The minimum length of the location.
                    /// </summary>
                    public const int MinLocationLength = 1;

                    /// <summary>
                    ///     The maximum length of the location.
                    /// </summary>
                    public const int MaxLocationLength = 100;
                }
            }

            /// <summary>
            ///     Represents limits for guild welcome screens.
            /// </summary>
            public static class WelcomeScreen
            {
                /// <summary>
                ///     Represents limits for guild welcome screen channels.
                /// </summary>
                public static class Channel
                {
                    /// <summary>
                    ///     The minimum length of the description.
                    /// </summary>
                    public const int MinDescriptionLength = 1;

                    /// <summary>
                    ///     The maximum length of the description.
                    /// </summary>
                    public const int MaxDescriptionLength = 32;
                }
            }
        }
    }
}

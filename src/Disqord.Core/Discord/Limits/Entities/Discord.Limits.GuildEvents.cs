namespace Disqord
{
    public static partial class Discord
    {
        public static partial class Limits
        {
            /// <summary>
            ///     Represents limits for guild events.
            /// </summary>
            public static class GuildEvents
            {
                /// <summary>
                ///     The minimum length of names.
                /// </summary>
                public const int MinNameLength = 1;

                /// <summary>
                ///     The maximum length of names.
                /// </summary>
                public const int MaxNameLength = 100;

                /// <summary>
                ///     The minimum length of descriptions.
                /// </summary>
                public const int MinDescriptionLength = 1;

                /// <summary>
                ///     The maximum length of descriptions.
                /// </summary>
                public const int MaxDescriptionLength = 1000;

                /// <summary>
                ///     Represents limits for guild event metadata.
                /// </summary>
                public static class Metadata
                {
                    /// <summary>
                    ///     The minimum length of locations.
                    /// </summary>
                    public const int MinLocationLength = 1;

                    /// <summary>
                    ///     The maximum length of locations.
                    /// </summary>
                    public const int MaxLocationLength = 100;
                }
            }
        }
    }
}

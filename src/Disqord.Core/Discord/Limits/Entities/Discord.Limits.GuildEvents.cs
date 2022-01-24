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
                public const int MinNameLength = 1;

                public const int MaxNameLength = 100;

                public const int MinDescriptionLength = 1;

                public const int MaxDescriptionLength = 1000;

                public static class Metadata
                {
                    public const int MinLocationLength = 1;

                    public const int MaxLocationLength = 100;
                }
            }
        }
    }
}

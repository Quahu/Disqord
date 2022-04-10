namespace Disqord
{
    public static partial class Discord
    {
        public static partial class Limits
        {
            /// <summary>
            ///     Represents limits for application commands.
            /// </summary>
            public static class ApplicationCommands
            {
                public const int MinNameLength = 1;

                public const int MaxNameLength = 32;

                public const int MinDescriptionLength = 1;

                public const int MaxDescriptionLength = 100;

                public const int MaxOptionAmount = 25;

                public static class Options
                {
                    public const int MinNameLength = 1;

                    public const int MaxNameLength = 32;

                    public const int MinDescriptionLength = 1;

                    public const int MaxDescriptionLength = 100;

                    public const int MaxChoiceAmount = 25;

                    public static class Choices
                    {
                        public const int MinNameLength = 1;

                        public const int MaxNameLength = 100;

                        public const long MaxIntegralValue = 9007199254740991;

                        public const long MinIntegralValue = -9007199254740991;

                        public const int MaxStringValueLength = 100;
                    }
                }
            }
        }
    }
}

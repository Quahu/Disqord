namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for application commands.
        /// </summary>
        public static class ApplicationCommand
        {
            /// <summary>
            ///     The minimum length of names.
            /// </summary>
            public const int MinNameLength = 1;

            /// <summary>
            ///     The maximum length of names.
            /// </summary>
            public const int MaxNameLength = 32;

            /// <summary>
            ///     The minimum length of descriptions.
            /// </summary>
            public const int MinDescriptionLength = 1;

            /// <summary>
            ///     The maximum length of descriptions.
            /// </summary>
            public const int MaxDescriptionLength = 100;

            /// <summary>
            ///     The maximum amount of options.
            /// </summary>
            public const int MaxOptionAmount = 25;

            /// <summary>
            ///     Represents limits for application command options.
            /// </summary>
            public static class Option
            {
                /// <summary>
                ///     The minimum length of names.
                /// </summary>
                public const int MinNameLength = 1;

                /// <summary>
                ///     The maximum length of names.
                /// </summary>
                public const int MaxNameLength = 32;

                /// <summary>
                ///     The minimum length of descriptions.
                /// </summary>
                public const int MinDescriptionLength = 1;

                /// <summary>
                ///     The maximum length of descriptions.
                /// </summary>
                public const int MaxDescriptionLength = 100;

                /// <summary>
                ///     The maximum amount of choices.
                /// </summary>
                public const int MaxChoiceAmount = 25;

                /// <summary>
                ///     Represents limits for application command option choices.
                /// </summary>
                public static class Choice
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
                    ///     The minimum integral value.
                    /// </summary>
                    public const long MinIntegralValue = -9007199254740991;

                    /// <summary>
                    ///     The maximum integral value.
                    /// </summary>
                    public const long MaxIntegralValue = 9007199254740991;

                    /// <summary>
                    ///     The maximum string value length.
                    /// </summary>
                    public const int MaxStringValueLength = 100;
                }
            }
        }
    }
}

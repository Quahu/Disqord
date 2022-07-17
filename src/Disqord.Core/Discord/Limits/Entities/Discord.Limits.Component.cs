namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for components.
        /// </summary>
        public static class Component
        {
            /// <summary>
            ///     The maximum length of custom IDs.
            /// </summary>
            public const int MaxCustomIdLength = 100;

            /// <summary>
            ///     Represents limits for button components.
            /// </summary>
            public static class Button
            {
                /// <summary>
                ///     The maximum length of labels.
                /// </summary>
                public const int MaxLabelLength = 80;
            }

            /// <summary>
            ///     Represents limits for selection components.
            /// </summary>
            public static class Selection
            {
                /// <summary>
                ///     The maximum length of placeholders.
                /// </summary>
                public const int MaxPlaceholderLength = 150;

                /// <summary>
                ///     The minimum value of minimum selected options.
                /// </summary>
                public const int MinMinimumSelectedOptions = 0;

                /// <summary>
                ///     The maximum value of minimum selected options.
                /// </summary>
                public const int MaxMinimumSelectedOptions = 25;

                /// <summary>
                ///     The minimum value of maximum selected options.
                /// </summary>
                public const int MinMaximumSelectedOptions = 1;

                /// <summary>
                ///     The maximum value of maximum selected options.
                /// </summary>
                public const int MaxMaximumSelectedOptions = 25;

                /// <summary>
                ///     The maximum amount of options.
                /// </summary>
                public const int MaxOptionAmount = 25;

                /// <summary>
                ///     Represents limits for selection component options.
                /// </summary>
                public class Option
                {
                    /// <summary>
                    ///     The maximum length of labels.
                    /// </summary>
                    public const int MaxLabelLength = 100;

                    /// <summary>
                    ///     The maximum length of values.
                    /// </summary>
                    public const int MaxValueLength = 100;

                    /// <summary>
                    ///     The maximum length of descriptions.
                    /// </summary>
                    public const int MaxDescriptionLength = 100;
                }
            }

            /// <summary>
            ///     Represents limits for text input components.
            /// </summary>
            public static class TextInput
            {
                /// <summary>
                ///     The minimum length of labels.
                /// </summary>
                public const int MinLabelLength = 1;

                /// <summary>
                ///     The maximum length of labels.
                /// </summary>
                public const int MaxLabelLength = 40;

                /// <summary>
                ///     The minimum value of minimum input length.
                /// </summary>
                public const int MinMinimumInputLength = 0;

                /// <summary>
                ///     The maximum value of minimum input length.
                /// </summary>
                public const int MaxMinimumInputLength = 4000;

                /// <summary>
                ///     The minimum value of maximum input length.
                /// </summary>
                public const int MinMaximumInputLength = 1;

                /// <summary>
                ///     The maximum value of maximum input length.
                /// </summary>
                public const int MaxMaximumInputLength = 4000;

                /// <summary>
                ///     The maximum length of prefilled values.
                /// </summary>
                public const int MaxPrefilledValueLength = 4000;

                /// <summary>
                ///     The maximum length of placeholders.
                /// </summary>
                public const int MaxPlaceholderLength = 100;
            }
        }
    }
}

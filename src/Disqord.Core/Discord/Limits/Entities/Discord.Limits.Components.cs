namespace Disqord
{
    public static partial class Discord
    {
        public static partial class Limits
        {
            public static class Components
            {
                public const int MaxCustomIdLength = 100;

                public static class Button
                {
                    public const int MaxLabelLength = 80;
                }

                public static class Selection
                {
                    public const int MaxPlaceholderLength = 150;

                    public const int MaxOptionsAmount = 25;

                    public const int MinMinimumValues = 0;

                    public const int MaxMinimumValues = 25;

                    public const int MinMaximumValues = 1;

                    public const int MaxMaximumValues = 25;

                    public class Option
                    {
                        public const int MaxLabelLength = 100;

                        public const int MaxValueLength = 100;

                        public const int MaxDescriptionLength = 100;
                    }
                }

                public static class TextInput
                {
                    public const int MinLabelLength = 1;

                    public const int MaxLabelLength = 40;

                    public const int MinMinimumInputLength = 0;

                    public const int MaxMinimumInputLength = 4000;

                    public const int MinMaximumInputLength = 1;

                    public const int MaxMaximumInputLength = 4000;

                    public const int MaxPrefilledValueLength = 4000;

                    public const int MaxPlaceholderLength = 100;
                }

            }
        }
    }
}

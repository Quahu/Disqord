namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for auto moderation rules.
        /// </summary>
        public static class AutoModerationRule
        {
            public const int MaxExemptRoleAmount = 20;

            public const int MaxExemptChannelAmount = 50;

            public static class TriggerMetadata
            {
                public const int MaxKeywordFilterAmount = 1000;

                public const int MaxKeywordFilterLength = 60;

                public const int MaxRegexPatternsAmount = 10;

                public const int MaxRegexPatternLength = 260;

                public const int MaxKeywordAllowListAmount = 100;

                public const int MaxKeywordPresetAllowListAmount = 1000;

                public const int MaxKeywordAllowedSubStringLength = 60;

                public const int MaxKeywordPresetAllowedSubStringLength = 60;

                public const int MaxMentionLimit = 50;
            }
            
            public static class ActionMetadata
            {
                public const int MaxCustomMessageLength = 150;
                public const int MaxDurationSeconds = 2419200;
            }
        }
    }
}

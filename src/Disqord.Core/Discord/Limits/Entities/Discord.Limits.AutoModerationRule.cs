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
            /// <summary>
            ///     The maximum amount of exempt roles.
            /// </summary>
            public const int MaxExemptRoleAmount = 20;

            /// <summary>
            ///     The maximum amount of exempt channels.
            /// </summary>
            public const int MaxExemptChannelAmount = 50;

            /// <summary>
            ///     Represents limits for auto moderation rule trigger metadata.
            /// </summary>
            public static class TriggerMetadata
            {
                /// <summary>
                ///     The maximum amount of keyword filters.
                /// </summary>
                public const int MaxKeywordFilterAmount = 1000;

                /// <summary>
                ///     The maximum length of keyword filters.
                /// </summary>
                public const int MaxKeywordFilterLength = 60;

                /// <summary>
                ///     The maximum amount of regex patterns.
                /// </summary>
                public const int MaxRegexPatternsAmount = 10;

                /// <summary>
                ///     The maximum length of regex patterns.
                /// </summary>
                public const int MaxRegexPatternLength = 260;

                /// <summary>
                ///     The maximum amount of allow list substrings when the trigger type is <see cref="AutoModerationRuleTrigger.Keyword"/>.
                /// </summary>
                public const int MaxKeywordAllowListAmount = 100;

                /// <summary>
                ///     The maximum amount of allow list substrings when the trigger type is <see cref="AutoModerationRuleTrigger.KeywordPreset"/>.
                /// </summary>
                public const int MaxKeywordPresetAllowListAmount = 1000;

                /// <summary>
                ///     The maximum length of the allowed substring when the trigger type is <see cref="AutoModerationRuleTrigger.Keyword"/>.
                /// </summary>
                public const int MaxKeywordAllowedSubStringLength = 60;

                /// <summary>
                ///     The maximum length of the allowed substring when the trigger type is <see cref="AutoModerationRuleTrigger.KeywordPreset"/>.
                /// </summary>
                public const int MaxKeywordPresetAllowedSubStringLength = 60;

                /// <summary>
                ///     The maximum limit of mentions when the trigger type is <see cref="AutoModerationRuleTrigger.MentionSpam"/>.
                /// </summary>
                public const int MaxMentionLimit = 50;
            }

            /// <summary>
            ///     Represents limits for auto moderation rule action metadata.
            /// </summary>
            public static class ActionMetadata
            {
                /// <summary>
                ///     The maximum amount of a custom message.
                /// </summary>
                public const int MaxCustomMessageLength = 150;

                /// <summary>
                ///     The maximum duration of seconds when the action type is <see cref="AutoModerationActionType.Timeout"/>.
                /// </summary>
                public const int MaxDurationSeconds = 2419200;
            }
        }
    }
}

namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits for messages.
        /// </summary>
        public static class Message
        {
            /// <summary>
            ///     The maximum length of the content.
            /// </summary>
            public const int MaxContentLength = 2000;

            /// <summary>
            ///     The maximum length of embedded content,
            ///     i.e. the total length of all text inside embeds.
            /// </summary>
            public const int MaxEmbeddedContentLength = 6000;

            /// <summary>
            ///     The maximum length of the nonce.
            /// </summary>
            public const int MaxNonceLength = 25;

            /// <summary>
            ///     The maximum amount of embeds.
            /// </summary>
            public const int MaxEmbedAmount = 25;

            /// <summary>
            ///     Represents limits for message embeds.
            /// </summary>
            public static class Embed
            {
                /// <summary>
                ///     The maximum length of the title.
                /// </summary>
                public const int MaxTitleLength = 256;

                /// <summary>
                ///     The maximum length of the description.
                /// </summary>
                public const int MaxDescriptionLength = 4096;

                /// <summary>
                ///     The maximum amount of fields.
                /// </summary>
                public const int MaxFieldAmount = 25;

                /// <summary>
                ///     Represents limits for message embed footers.
                /// </summary>
                public static class Footer
                {
                    /// <summary>
                    ///     The maximum length of the text.
                    /// </summary>
                    public const int MaxTextLength = 2048;
                }

                /// <summary>
                ///     Represents limits for message embed authors.
                /// </summary>
                public static class Author
                {
                    /// <summary>
                    ///     The maximum length of the name.
                    /// </summary>
                    public const int MaxNameLength = 256;
                }

                /// <summary>
                ///     Represents limits for message embed fields.
                /// </summary>
                public static class Field
                {
                    /// <summary>
                    ///     The maximum length of the name.
                    /// </summary>
                    public const int MaxNameLength = 256;

                    /// <summary>
                    ///     The maximum length of the value.
                    /// </summary>
                    public const int MaxValueLength = 1024;
                }
            }

            /// <summary>
            ///     Represents limits for allowed mentions.
            /// </summary>
            public static class AllowedMentions
            {
                /// <summary>
                ///     The maximum amount of mentions.
                /// </summary>
                /// <remarks>
                ///     Mentioned users and roles are counted separately.
                /// </remarks>
                public const int MaxMentionAmount = 100;
            }
        }
    }
}

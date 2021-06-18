using System;
using System.Collections.Generic;
using System.Text;
using Disqord.Collections;

namespace Disqord
{
    /// <summary>
    ///     Represents utility methods related to Discord's Markdown.
    /// </summary>
    public static class Markdown
    {
        /// <summary>
        ///     The set containing the escaped markdown characters.
        /// </summary>
        public static readonly IReadOnlySet<char> EscapedCharacters = new HashSet<char>(5)
        {
            '\\',
            '*',
            '`',
            '_',
            '~'
        }.ReadOnly();

        // *text*
        public static string Italics(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Italics(value.ToString().AsSpan());
        }

        public static string Italics(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Italics(text.AsSpan());
        }

        public static string Italics(ReadOnlySpan<char> text)
            => string.Concat("*", text, "*");

        // **text**
        public static string Bold(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Bold(value.ToString().AsSpan());
        }

        public static string Bold(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Bold(text.AsSpan());
        }

        public static string Bold(ReadOnlySpan<char> text)
            => string.Concat("**", text, "**");

        // ***text***
        public static string BoldItalics(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return BoldItalics(value.ToString().AsSpan());
        }

        public static string BoldItalics(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return BoldItalics(text.AsSpan());
        }

        public static string BoldItalics(ReadOnlySpan<char> text)
            => string.Concat("***", text, "***");

        // __text__
        public static string Underline(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Underline(value.ToString().AsSpan());
        }

        public static string Underline(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Underline(text.AsSpan());
        }

        public static string Underline(ReadOnlySpan<char> text)
            => string.Concat("__", text, "__");

        // ~~text~~
        public static string Strikethrough(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Strikethrough(value.ToString().AsSpan());
        }

        public static string Strikethrough(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Strikethrough(text.AsSpan());
        }

        public static string Strikethrough(ReadOnlySpan<char> text)
            => string.Concat("~~", text, "~~");

        // [title](url)
        public static string Link(string title, string url)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return Link(title.AsSpan(), url.AsSpan());
        }

        public static string Link(ReadOnlySpan<char> title, ReadOnlySpan<char> url)
            => new StringBuilder().Append('[').Append(title).Append("](").Append(url).Append(')').ToString();

        // `code`
        public static string Code(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Code(value.ToString().AsSpan());
        }

        public static string Code(string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return Code(code.AsSpan());
        }

        public static string Code(ReadOnlySpan<char> code)
            => string.Concat("`", code, "`");

        // ```\ncode```
        public static string CodeBlock(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return CodeBlock(value.ToString().AsSpan());
        }

        public static string CodeBlock(string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return CodeBlock(code.AsSpan());
        }

        public static string CodeBlock(ReadOnlySpan<char> code)
            => string.Concat("```\n", code, "```");

        // ```language\ncode```
        public static string CodeBlock(string language, object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return CodeBlock(language, value.ToString());
        }

        public static string CodeBlock(string language, string code)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return CodeBlock(language.AsSpan(), code.AsSpan());
        }

        public static string CodeBlock(ReadOnlySpan<char> language, ReadOnlySpan<char> code)
            => new StringBuilder().Append("```").Append(language).Append('\n').Append(code).Append("```").ToString();

        public static string Escape(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Escape(text.AsSpan());
        }

        public static string Escape(ReadOnlySpan<char> text)
        {
            var builder = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                if (EscapedCharacters.Contains(character))
                    builder.Append('\\');

                builder.Append(character);
            }

            return builder.ToString();
        }

        public static string Timestamp(DateTimeOffset dateTimeOffset)
            => Timestamp(dateTimeOffset.ToUnixTimeSeconds());

        public static string Timestamp(long unixTimestamp)
            => string.Concat("<t:", unixTimestamp.ToString(), ">");

        public static string Timestamp(DateTimeOffset dateTimeOffset, TimestampFormat format)
            => Timestamp(dateTimeOffset.ToUnixTimeSeconds(), format);

        public static string Timestamp(long unixTimestamp, TimestampFormat format)
            => new StringBuilder().Append("<t:").Append(unixTimestamp).Append(':').Append((char) format).Append('>').ToString();

        public enum TimestampFormat
        {
            ShortTime = 't',

            LongTime = 'T',

            ShortDate = 'd',

            LongDate = 'D',

            ShortDateTime = 'f',

            LongDateTime = 'F',

            RelativeTime = 'R'
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Qommon.Collections;

namespace Disqord
{
    public static class Markdown
    {
        /// <summary>
        ///     The set containing the escaped markdown characters.
        /// </summary>
        public static readonly ReadOnlySet<char> EscapedCharacters = new ReadOnlySet<char>(new HashSet<char>(5)
        {
            '\\',
            '*',
            '`',
            '_',
            '~'
        });

        private const int STACK_TEXT_LENGTH = 2000;

        // *text*
        public static string Italics(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Italics(text.AsSpan());
        }

        public static string Italics(ReadOnlySpan<char> text)
        {
            var length = text.Length + 2;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '*';
            text.CopyTo(buffer.Slice(1));
            buffer[buffer.Length - 1] = '*';
            return new string(buffer);
        }

        // **text**
        public static string Bold(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Bold(text.AsSpan());
        }

        public static string Bold(ReadOnlySpan<char> text)
        {
            var length = text.Length + 4;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '*';
            buffer[1] = '*';
            text.CopyTo(buffer.Slice(2));
            buffer[buffer.Length - 2] = '*';
            buffer[buffer.Length - 1] = '*';
            return new string(buffer);
        }

        // ***text***
        public static string BoldItalics(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return BoldItalics(text.AsSpan());
        }

        public static string BoldItalics(ReadOnlySpan<char> text)
        {
            var length = text.Length + 6;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '*';
            buffer[1] = '*';
            buffer[2] = '*';
            text.CopyTo(buffer.Slice(3));
            buffer[buffer.Length - 3] = '*';
            buffer[buffer.Length - 2] = '*';
            buffer[buffer.Length - 1] = '*';
            return new string(buffer);
        }

        // __text__
        public static string Underline(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Underline(text.AsSpan());
        }

        public static string Underline(ReadOnlySpan<char> text)
        {
            var length = text.Length + 4;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '_';
            buffer[1] = '_';
            text.CopyTo(buffer.Slice(2));
            buffer[buffer.Length - 2] = '_';
            buffer[buffer.Length - 1] = '_';
            return new string(buffer);
        }

        // ~~text~~
        public static string Strikethrough(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return Strikethrough(text.AsSpan());
        }

        public static string Strikethrough(ReadOnlySpan<char> text)
        {
            var length = text.Length + 4;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '~';
            buffer[1] = '~';
            text.CopyTo(buffer.Slice(2));
            buffer[buffer.Length - 2] = '~';
            buffer[buffer.Length - 1] = '~';
            return new string(buffer);
        }

        // [title](url)_
        public static string Link(string title, string url)
            => $"[{title}]({url})";

        // `code`
        public static string Code(string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return Code(code.AsSpan());
        }

        public static string Code(ReadOnlySpan<char> code)
        {
            var length = code.Length + 2;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '`';
            code.CopyTo(buffer.Slice(1));
            buffer[buffer.Length - 1] = '`';
            return new string(buffer);
        }

        // ```\ncode```
        public static string CodeBlock(string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return CodeBlock(code.AsSpan());
        }

        public static string CodeBlock(ReadOnlySpan<char> code)
        {
            var length = code.Length + 7;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '`';
            buffer[1] = '`';
            buffer[2] = '`';
            buffer[3] = '\n';
            code.CopyTo(buffer.Slice(4));
            buffer[buffer.Length - 3] = '`';
            buffer[buffer.Length - 2] = '`';
            buffer[buffer.Length - 1] = '`';
            return new string(buffer);
        }

        // ```language\ncode```
        public static string CodeBlock(string language, string code)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (code == null)
                throw new ArgumentNullException(nameof(code));

            return CodeBlock(language.AsSpan(), code.AsSpan());
        }

        public static string CodeBlock(ReadOnlySpan<char> language, ReadOnlySpan<char> code)
        {
            var length = language.Length + code.Length + 7;
            Span<char> buffer = length > STACK_TEXT_LENGTH
                ? new char[length]
                : stackalloc char[length];

            buffer[0] = '`';
            buffer[1] = '`';
            buffer[2] = '`';
            language.CopyTo(buffer.Slice(3));
            buffer[3 + language.Length] = '\n';
            code.CopyTo(buffer.Slice(4 + language.Length));
            buffer[buffer.Length - 3] = '`';
            buffer[buffer.Length - 2] = '`';
            buffer[buffer.Length - 1] = '`';
            return new string(buffer);
        }

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
    }
}

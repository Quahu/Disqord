using System.Collections.Generic;
using System.Text;
using Qommon.Collections;

namespace Disqord
{
    public static class Markdown
    {
        public const char ZERO_WIDTH_SPACE = '\u200b';

        /// <summary>
        ///     The set containing the escaped markdown characters.
        /// </summary>
        public static readonly ReadOnlySet<char> Characters = new ReadOnlySet<char>(_characters);

        private static readonly HashSet<char> _characters = new HashSet<char>(5)
        {
            '\\',
            '*',
            '`',
            '_',
            '~'
        };

        public static string Italics(string text)
            => $"*{text}*";

        public static string Bold(string text)
            => $"**{text}**";

        public static string BoldItalics(string text)
            => $"***{text}***";

        public static string Underline(string text)
            => $"__{text}__";

        public static string Strikethrough(string text)
            => $"~~{text}~~";

        public static string Link(string title, string url)
            => $"[{title}]({url})";

        public static string Code(string code)
            => $"`{code}`";

        public static string CodeBlock(string code)
            => $"```\n{code}```";

        public static string CodeBlock(string language, string code)
            => $"```{language}\n{code}```";

        public static string Escape(string text)
        {
            var builder = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                if (_characters.Contains(character))
                    builder.Append('\\');

                builder.Append(character);
            }

            return builder.ToString();
        }
    }
}

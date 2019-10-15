namespace Disqord
{
    public static class Markdown
    {
        public const char ZERO_WIDTH_SPACE = '\u200b';

        private static readonly string[] _markdown = { "\\", "*", "`", "_", "~", "||" };

        public static string Italics(string text)
            => $"*{text}*";

        public static string Bold(string text)
            => $"**{text}**";

        public static string BoldItalics(string text)
            => $"***{text}***";

        public static string Underline(string text)
            => $"__{text}__";

        public static string CrossOut(string text)
            => $"~~{text}~~";

        public static string MaskedUrl(string title, string url)
            => $"[{title}]({url})";

        public static string InlineCode(string code)
            => $"`{code}`";

        public static string CodeBlock(string code)
            => $"```\n{code}```";

        public static string CodeBlock(string language, string code)
            => $"```{language}\n{code}```";

        public static string EscapeMarkdown(string text)
        {
            for (var i = 0; i < _markdown.Length; i++)
            {
                var markdown = _markdown[i];
                text = text.Replace(markdown, $"\\{markdown}");
            }

            return text;
        }
    }
}

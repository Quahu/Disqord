using System;

namespace Disqord.Bot.Prefixes
{
    public sealed class MentionPrefix : IPrefix
    {
        public static readonly MentionPrefix Instance = new MentionPrefix();

        private MentionPrefix()
        { }

        public bool TryFind(CachedUserMessage message, out string output)
        {
            var contentSpan = message.Content.AsSpan();
            if (contentSpan.Length > 17
                && contentSpan[0] == '<'
                && contentSpan[1] == '@')
            {
                var closingBracketIndex = contentSpan.IndexOf('>');
                if (closingBracketIndex != -1)
                {
                    var idSpan = contentSpan[2] == '!'
                            ? contentSpan.Slice(3, closingBracketIndex - 3)
                            : contentSpan.Slice(2, closingBracketIndex - 2);
                    if (Snowflake.TryParse(idSpan, out var id) && id == message.Client.CurrentUser.Id)
                    {
                        output = new string(contentSpan.Slice(closingBracketIndex + 1));
                        return true;
                    }
                }
            }

            output = null;
            return false;
        }

        public override int GetHashCode()
            => -1;

        public override bool Equals(object obj)
            => obj is MentionPrefix;

        public override string ToString()
            => "<mention>";
    }
}

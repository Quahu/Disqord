using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class LocalCustomEmojiParser : TypeParser<LocalCustomEmoji>
    {
        public static LocalCustomEmojiParser Instance => _instance ?? (_instance = new LocalCustomEmojiParser());

        private static LocalCustomEmojiParser _instance;

        private LocalCustomEmojiParser()
        { }

        public override ValueTask<TypeParserResult<LocalCustomEmoji>> ParseAsync(Parameter parameter, string value, CommandContext context)
            => LocalCustomEmoji.TryParse(value, out var emoji)
                ? TypeParserResult<LocalCustomEmoji>.Successful(emoji)
                : TypeParserResult<LocalCustomEmoji>.Unsuccessful("Invalid custom emoji format.");
    }
}

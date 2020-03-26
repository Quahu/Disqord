using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class LocalCustomEmojiTypeParser : TypeParser<LocalCustomEmoji>
    {
        public static LocalCustomEmojiTypeParser Instance => _instance ?? (_instance = new LocalCustomEmojiTypeParser());

        private static LocalCustomEmojiTypeParser _instance;

        private LocalCustomEmojiTypeParser()
        { }

        public override ValueTask<TypeParserResult<LocalCustomEmoji>> ParseAsync(Parameter parameter, string value, CommandContext context)
            => LocalCustomEmoji.TryParse(value, out var emoji)
                ? TypeParserResult<LocalCustomEmoji>.Successful(emoji)
                : TypeParserResult<LocalCustomEmoji>.Unsuccessful("Invalid custom emoji format.");
    }
}

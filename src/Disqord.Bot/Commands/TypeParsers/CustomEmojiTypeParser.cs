using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public class CustomEmojiTypeParser : DiscordTypeParser<ICustomEmoji>
    {
        public override ValueTask<TypeParserResult<ICustomEmoji>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            if (LocalCustomEmoji.TryParse(value, out var emoji))
                return Success(emoji);

            return Failure("Invalid custom emoji.");
        }
    }
}

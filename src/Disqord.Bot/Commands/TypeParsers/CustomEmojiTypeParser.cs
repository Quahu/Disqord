using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    /// <summary>
    ///     Represents type parsing for the <see cref="ICustomEmoji"/> type
    ///     via <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>.
    /// </summary>
    public class CustomEmojiTypeParser : DiscordTypeParser<ICustomEmoji>
    {
        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<ICustomEmoji>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            if (LocalCustomEmoji.TryParse(value, out var emoji))
                return Success(emoji);

            return Failure("Invalid custom emoji.");
        }
    }
}

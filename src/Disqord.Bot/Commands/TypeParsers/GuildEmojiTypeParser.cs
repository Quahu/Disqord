using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    /// <summary>
    ///     Represents type parsing for the <see cref="ICustomEmoji"/> type
    ///     via <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>.
    /// </summary>
    public class GuildEmojiTypeParser : DiscordGuildTypeParser<IGuildEmoji>
    {
        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<IGuildEmoji>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (LocalCustomEmoji.TryParse(value, out var emoji))
            {
                if (context.Guild.Emojis.TryGetValue(emoji.Id, out var guildEmoji))
                    return Success(guildEmoji);

                return Failure("The provided custom emoji is not from this guild.");
            }

            return Failure("Invalid custom emoji.");
        }
    }
}

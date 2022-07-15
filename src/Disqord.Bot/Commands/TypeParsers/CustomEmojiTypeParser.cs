using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands.Parsers;

/// <summary>
///     Represents type parsing for the <see cref="ICustomEmoji"/> type
///     via <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>.
/// </summary>
public class CustomEmojiTypeParser : DiscordTypeParser<ICustomEmoji>
{
    /// <inheritdoc/>
    public override ValueTask<ITypeParserResult<ICustomEmoji>> ParseAsync(IDiscordCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (LocalCustomEmoji.TryParse(value.Span, out var emoji))
            return Success(emoji);

        return Failure("Invalid custom emoji.");
    }
}

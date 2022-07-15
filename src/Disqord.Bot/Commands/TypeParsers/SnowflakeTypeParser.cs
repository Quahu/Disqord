using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands.Parsers;

/// <summary>
///     Represents type parsing for the <see cref="Snowflake"/> type
///     via <see cref="Snowflake.TryParse(string, out Snowflake)"/>.
/// </summary>
public class SnowflakeTypeParser : DiscordTypeParser<Snowflake>
{
    /// <inheritdoc/>
    public override ValueTask<ITypeParserResult<Snowflake>> ParseAsync(IDiscordCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (Snowflake.TryParse(value.Span, out var result))
            return Success(result);

        return Failure("Invalid ID.");
    }
}

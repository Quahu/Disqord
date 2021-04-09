using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    /// <summary>
    ///     Represents type parsing for the <see cref="Snowflake"/> type
    ///     via <see cref="Snowflake.TryParse(string, out Snowflake)"/>.
    /// </summary>
    public class SnowflakeTypeParser : DiscordTypeParser<Snowflake>
    {
        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<Snowflake>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            if (Snowflake.TryParse(value, out var result))
                return Success(result);

            return Failure("Invalid ID.");
        }
    }
}

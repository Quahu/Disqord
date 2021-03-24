using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public class SnowflakeTypeParser : TypeParser<Snowflake>
    {
        public override ValueTask<TypeParserResult<Snowflake>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            if (Snowflake.TryParse(value, out var result))
                return Success(result);

            return Failure("Invalid ID.");
        }
    }
}

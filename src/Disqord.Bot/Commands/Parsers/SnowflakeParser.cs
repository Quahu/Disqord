using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class SnowflakeParser : TypeParser<Snowflake>
    {
        public static SnowflakeParser Instance => _instance ?? (_instance = new SnowflakeParser());

        private static SnowflakeParser _instance;

        private SnowflakeParser()
        { }

        public override ValueTask<TypeParserResult<Snowflake>> ParseAsync(Parameter parameter, string value, CommandContext context)
            => Snowflake.TryParse(value, out var snowflake)
                ? TypeParserResult<Snowflake>.Successful(snowflake)
                : TypeParserResult<Snowflake>.Unsuccessful("Invalid snowflake format.");
    }
}

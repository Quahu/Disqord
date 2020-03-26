using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class SnowflakeTypeParser : TypeParser<Snowflake>
    {
        public static SnowflakeTypeParser Instance => _instance ?? (_instance = new SnowflakeTypeParser());

        private static SnowflakeTypeParser _instance;

        private SnowflakeTypeParser()
        { }

        public override ValueTask<TypeParserResult<Snowflake>> ParseAsync(Parameter parameter, string value, CommandContext context)
            => Snowflake.TryParse(value, out var snowflake)
                ? TypeParserResult<Snowflake>.Successful(snowflake)
                : TypeParserResult<Snowflake>.Unsuccessful("Invalid snowflake format.");
    }
}

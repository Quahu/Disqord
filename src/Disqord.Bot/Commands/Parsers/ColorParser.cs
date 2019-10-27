using System;
using System.Globalization;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class ColorParser : TypeParser<Color>
    {
        public static ColorParser Instance => _instance ?? (_instance = new ColorParser());

        private static ColorParser _instance;

        private ColorParser()
        { }

        public override ValueTask<TypeParserResult<Color>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var valueSpan = value.AsSpan();
            if (valueSpan.Length > 2)
            {
                var valid = false;
                if (valueSpan[0] == '0' && (valueSpan[1] == 'x' || valueSpan[1] == 'X') && valueSpan.Length == 8)
                {
                    valid = true;
                    valueSpan = valueSpan.Slice(2);
                }
                else if (value[0] == '#' && value.Length == 7)
                {
                    valid = true;
                    valueSpan = valueSpan.Slice(1);
                }

                if (valid && int.TryParse(valueSpan, NumberStyles.HexNumber, null, out var result))
                    return TypeParserResult<Color>.Successful(result);
            }

            return TypeParserResult<Color>.Unsuccessful("Invalid color hex value provided.");
        }
    }
}

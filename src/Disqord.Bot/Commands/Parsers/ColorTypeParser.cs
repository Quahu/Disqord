using System;
using System.Globalization;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class ColorTypeParser : TypeParser<Color>
    {
        public static ColorTypeParser Instance => _instance ?? (_instance = new ColorTypeParser());

        private static ColorTypeParser _instance;

        private ColorTypeParser()
        { }

        public override ValueTask<TypeParserResult<Color>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            if (value.Length > 2)
            {
                var valueSpan = value.AsSpan();
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

                if (valid && uint.TryParse(valueSpan, NumberStyles.HexNumber, null, out var result))
                    return TypeParserResult<Color>.Successful((int) result);
            }

            return TypeParserResult<Color>.Unsuccessful("Invalid color hex value.");
        }
    }
}

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
            if (value.Length > 2)
            {
                var valid = false;
                if (value[0] == '0' && (value[1] == 'x' || value[1] == 'X') && value.Length == 8)
                {
                    valid = true;
                    value = value.Substring(2);
                }

                else if (value[0] == '#' && value.Length == 7)
                {
                    valid = true;
                    value = value.Substring(1);
                }

                if (valid && int.TryParse(value, NumberStyles.HexNumber, null, out var result))
                    return TypeParserResult<Color>.Successful(result);
            }

            return TypeParserResult<Color>.Unsuccessful("Invalid color hex value provided.");
        }
    }
}

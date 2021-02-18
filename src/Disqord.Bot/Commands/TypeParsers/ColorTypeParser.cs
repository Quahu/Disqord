using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class ColorTypeParser : TypeParser<Color>
    {
        private readonly bool _allowProperties;
        private readonly bool _allowSpaces;
        private readonly StringComparison _comparison;

        // SpringGreen -> Color.SpringGreen
        private readonly Dictionary<string, Color> _colors;

        // Spring Green -> SpringGreen
        private readonly Dictionary<string, string> _spacedNames;

        public ColorTypeParser(bool allowProperties = true, bool allowSpaces = true, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _allowProperties = allowProperties;
            _allowSpaces = allowSpaces;
            _comparison = comparison;

            if (!_allowProperties)
                return;

            _colors = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(x => x.Name, x => (Color) x.GetValue(null), StringComparer.FromComparison(_comparison));

            if (!_allowSpaces)
                return;

            _spacedNames = new Dictionary<string, string>(_colors.Count, _colors.Comparer);
            var builder = new StringBuilder();
            foreach (var name in _colors.Keys)
            {
                builder.Clear();
                for (var i = 0; i < name.Length; i++)
                {
                    var character = name[i];
                    if (i != 0 && char.IsUpper(character))
                        builder.Append(' ');

                    builder.Append(character);
                }

                _spacedNames[builder.ToString()] = name;
            }
        }

        public override ValueTask<TypeParserResult<Color>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            if (value.Length > 2)
            {
                var valueSpan = value.AsSpan();
                switch (valueSpan[0])
                {
                    case '0' when valueSpan[1] == 'x' || valueSpan[1] == 'X' && valueSpan.Length == 8:
                    {
                        valueSpan = valueSpan[2..];
                        break;
                    }
                    case '#' when value.Length == 7:
                    {
                        valueSpan = valueSpan[1..];
                        break;
                    }
                }

                if (uint.TryParse(valueSpan, NumberStyles.HexNumber, null, out var result))
                    return Success((int) result);

                if (_allowProperties)
                {
                    string name;
                    if (_allowSpaces && value.IndexOf(' ') != -1)
                    {
                        name = _spacedNames.GetValueOrDefault(value);
                    }
                    else
                    {
                        name = value;
                    }

                    if (name != null && _colors.TryGetValue(name, out var color))
                        return Success(color);
                }
            }

            return Failure("Invalid color value.");
        }
    }
}

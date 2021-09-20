using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    /// <summary>
    ///     Represents type parsing for the <see cref="Color"/> type.
    ///     The accepted inputs can be customised via the constructor.
    /// </summary>
    /// <remarks>
    ///     Supports the following inputs, in order:
    ///     <list type="number">
    ///         <item>
    ///             <term> Hex </term>
    ///             <description>
    ///                 The hexadecimal value of the color.
    ///                 Must be prefixed with either <c>#</c> or <c>0x</c>.
    ///                 The fixed-length after the given prefix must be:
    ///                 <list type="table">
    ///                     <item>
    ///                         <term> # </term>
    ///                         <description> 3 or 6 characters. </description>
    ///                     </item>
    ///                     <item>
    ///                         <term> 0x </term>
    ///                         <description> 6 characters. </description>
    ///                     </item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term> Raw </term>
    ///             <description> The raw value of the color. </description>
    ///         </item>
    ///         <item>
    ///             <term> Name </term>
    ///             <description> The name of the color. This is case-insensitive. </description>
    ///         </item>
    ///         <item>
    ///             <term> Random </term>
    ///             <description> <c>random</c> producing a random color value. This is case-insensitive. </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class ColorTypeParser : DiscordTypeParser<Color>
    {
        /// <summary>
        ///     Gets the dictionary of names mapped to their respective colors.
        ///     E.g. <c>Red</c> -> <see cref="Color.Red"/>.
        /// </summary>
        public IReadOnlyDictionary<string, Color> Colors => _colors;

        /// <summary>
        ///     Gets the dictionary of spaced names to their respective non-spaced names.
        ///     E.g. <c>Dark Red</c> -> <c>DarkRed</c>.
        /// </summary>
        public IReadOnlyDictionary<string, string> SpacedNames => _spacedNames;

        private readonly bool _allowProperties;
        private readonly bool _allowRandom;
        private readonly bool _allowSpaces;

        // "SpringGreen" -> Color.SpringGreen
        private readonly Dictionary<string, Color> _colors;

        // "Spring Green" -> "SpringGreen"
        private readonly Dictionary<string, string> _spacedNames;

        public ColorTypeParser(bool allowProperties = true, bool allowRandom = true, bool allowSpaces = true)
        {
            _allowProperties = allowProperties;
            _allowRandom = allowRandom;
            _allowSpaces = allowSpaces;

            if (!_allowProperties)
                return;

            _colors = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name != nameof(Color.Random))
                .ToDictionary(x => x.Name, x => (Color) x.GetValue(null), StringComparer.OrdinalIgnoreCase);
            _colors.TrimExcess();

            if (!_allowSpaces)
                return;

            _spacedNames = new Dictionary<string, string>(_colors.Count, _colors.Comparer);
            var builder = new StringBuilder();
            foreach (var name in _colors.Keys)
            {
                builder.Clear();
                var hasSpace = false;
                for (var i = 0; i < name.Length; i++)
                {
                    var character = name[i];
                    if (i != 0 && char.IsUpper(character))
                    {
                        hasSpace = true;
                        builder.Append(' ');
                    }

                    builder.Append(character);
                }


                if (hasSpace)
                    _spacedNames[builder.ToString()] = name;
            }
            _spacedNames.TrimExcess();
        }

        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<Color>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            // Checks for the following hex formats:
            // #FFF, #FFFFFF, 0xFFFFFF
            if (value.Length > 2
                && (value[0] == '#' && (value.Length == 4 || value.Length == 7)
                || value[0] == '0' && (value[1] == 'x' || value[1] == 'X') && value.Length <= 8))
            {
                var valueSpan = value.AsSpan();
                if (valueSpan[0] == '#')
                {
                    valueSpan = valueSpan[1..];
                    if (valueSpan.Length == 3)
                    {
                        // 123 -> 112233
                        valueSpan = new[]
                        {
                            valueSpan[0], valueSpan[0],
                            valueSpan[1], valueSpan[1],
                            valueSpan[2], valueSpan[2]
                        };
                    }
                }
                else
                {
                    valueSpan = valueSpan[2..];
                }

                if (uint.TryParse(valueSpan, NumberStyles.HexNumber, null, out var result))
                    return Success((int) result);
            }
            else if (uint.TryParse(value, out var rawValue))
            {
                if (rawValue <= 0xFFFFFF)
                    return Success((int) rawValue);
            }
            else if (_allowProperties)
            {
                if (_allowRandom && value.Equals("random", StringComparison.OrdinalIgnoreCase))
                    return Success(Color.Random);

                // If spaces are allowed we look up the actual name with the space-separated name.
                // We also trim any extra spaces.
                value = _allowSpaces && value.IndexOf(' ') != -1
                    ? _spacedNames.GetValueOrDefault(string.Join(' ', value.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                    : value;

                if (value != null && _colors.TryGetValue(value, out var color))
                    return Success(color);
            }

            return Failure("Invalid color value.");
        }
    }
}

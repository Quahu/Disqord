using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents the formatter used with <see cref="string.Format(IFormatProvider?, string, object?[])"/> to format routes with arguments and map them name -> value.
    /// </summary>
    public class RouteFormatter : IFormatProvider, ICustomFormatter
    {
        protected readonly Dictionary<string, object> Parameters;

        protected RouteFormatter()
        {
            Parameters = new Dictionary<string, object>();
        }

        public static FormattedRoute Format(Route route, object[] arguments = null, IEnumerable<KeyValuePair<string, object>> queryParameters = null)
        {
            var formatter = new RouteFormatter();
            var builder = new StringBuilder();
            // We use the formatter with string.Format() and it'll collect all encountered parameters with their names.
            builder.AppendFormat(formatter, route.Path, arguments ?? Array.Empty<object>());

            if (queryParameters != null)
            {
                var first = true;
                foreach (var parameter in queryParameters)
                {
                    if (string.IsNullOrWhiteSpace(parameter.Key))
                        throw new ArgumentException("Query parameters must not contain null or whitespace keys.");

                    var value = parameter.Value;
                    if (value == null)
                        throw new ArgumentException("Query parameters must not contain null values.");

                    builder.Append(first ? '?' : '&');
                    builder.Append(parameter.Key);
                    builder.Append('=');
                    if (value is IEnumerable enumerable and not string)
                    {
                        builder.AppendJoin(',', enumerable.Cast<object>());
                    }
                    else
                    {
                        builder.Append(Uri.EscapeDataString(value.ToString()));
                    }
                    first = false;
                }
            }

            return new FormattedRoute(route, builder.ToString(), new RouteParameters(formatter.Parameters));
        }

        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }

        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new FormatException($"No format specified for route parameter '{arg}'.");

            if (!Parameters.TryAdd(format, arg))
                throw new FormatException($"Multiple route parameters with the name '{format}' found.");

            var argString = arg.ToString();
            if (string.IsNullOrWhiteSpace(argString))
                throw new FormatException("The format argument must not return a null or whitespace string.");

            return Uri.EscapeDataString(argString);
        }
    }
}

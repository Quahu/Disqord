using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qommon;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents the formatter used with <see cref="string.Format(IFormatProvider?, string, object?[])"/> to format routes with arguments and map them name -> value.
/// </summary>
public class RouteFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    ///     The parameters parsed so far.
    /// </summary>
    protected readonly Dictionary<string, object> Parameters;

    /// <summary>
    ///     Instantiates a new <see cref="RouteFormatter"/>.
    /// </summary>
    public RouteFormatter()
    {
        Parameters = new Dictionary<string, object>();
    }

    /// <summary>
    ///     Formats the provided route with the specified arguments and query parameters.
    /// </summary>
    /// <param name="route"> The route to format. </param>
    /// <param name="arguments"> The positional route arguments. </param>
    /// <param name="queryParameters"> The query parameters. </param>
    /// <returns>
    ///     The formatted route.
    /// </returns>
    public virtual IFormattedRoute Format(IFormattableRoute route,
        object[]? arguments = null, IEnumerable<KeyValuePair<string, object>>? queryParameters = null)
    {
        var builder = new StringBuilder();

        // We use the formatter with string.Format() and it'll collect all encountered parameters with their names.
        builder.AppendFormat(this, route.Path, arguments ?? Array.Empty<object>());

        if (queryParameters != null)
        {
            var first = true;
            foreach (var parameter in queryParameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Key))
                    Throw.ArgumentException("Query parameters must not contain null or whitespace keys.");

                var value = parameter.Value;
                if (value == null)
                    Throw.ArgumentException("Query parameters must not contain null values.");

                builder.Append(first ? '?' : '&');
                builder.Append(parameter.Key);
                builder.Append('=');
                if (value is IEnumerable enumerable and not string)
                {
                    builder.AppendJoin(',', enumerable.Cast<object>());
                }
                else
                {
                    builder.Append(Uri.EscapeDataString(value.ToString()!));
                }

                first = false;
            }
        }

        return new FormattedRoute(route, builder.ToString(), new RouteParameters(Parameters));
    }

    object? IFormatProvider.GetFormat(Type? formatType)
    {
        if (formatType == typeof(ICustomFormatter))
            return this;

        return null;
    }

    string ICustomFormatter.Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        Guard.IsNotNull(arg);

        if (string.IsNullOrWhiteSpace(format))
            Throw.FormatException($"No format specified for route parameter '{arg}'.");

        if (!Parameters.TryAdd(format, arg))
            Throw.FormatException($"Multiple route parameters with the name '{format}' found.");

        var argString = arg.ToString();
        if (string.IsNullOrWhiteSpace(argString))
            Throw.FormatException("The format argument must not return a null or whitespace string.");

        return Uri.EscapeDataString(argString);
    }
}

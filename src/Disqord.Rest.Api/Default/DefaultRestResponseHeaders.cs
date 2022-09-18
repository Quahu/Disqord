using System;
using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord.Rest.Api.Default;

/// <summary>
///     Represents the known response headers for REST requests
///     that are used throughout the default components.
/// </summary>
public readonly struct DefaultRestResponseHeaders
{
    /// <inheritdoc cref="RestApiHeaderNames.RetryAfter"/>
    public Optional<TimeSpan> RetryAfter => GetHeader(RestApiHeaderNames.RetryAfter, value => TimeSpan.FromSeconds(int.Parse(value)));

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitGlobal"/>
    public Optional<bool> IsGlobal => GetHeader(RestApiHeaderNames.RateLimitGlobal, bool.Parse);

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitLimit"/>
    public Optional<int> Limit => GetHeader(RestApiHeaderNames.RateLimitLimit, int.Parse);

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitRemaining"/>
    public Optional<int> Remaining => GetHeader(RestApiHeaderNames.RateLimitRemaining, int.Parse);

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitReset"/>
    public Optional<DateTimeOffset> ResetsAt => GetHeader(RestApiHeaderNames.RateLimitReset, value => DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(ParseDouble(value)));

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitResetAfter"/>
    public Optional<TimeSpan> ResetsAfter => GetHeader(RestApiHeaderNames.RateLimitResetAfter, value => TimeSpan.FromSeconds(ParseDouble(value)));

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitBucket"/>
    public Optional<string> Bucket => GetHeader(RestApiHeaderNames.RateLimitBucket);

    /// <inheritdoc cref="RestApiHeaderNames.RateLimitScope"/>
    public Optional<string> Scope => GetHeader(RestApiHeaderNames.RateLimitScope);

    /// <inheritdoc cref="RestApiHeaderNames.Date"/>
    public Optional<DateTimeOffset> Date => GetHeader(RestApiHeaderNames.Date, DateTimeOffset.Parse);

    private readonly IDictionary<string, string> _headers;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultRestResponseHeaders"/>.
    /// </summary>
    /// <param name="headers"> The headers to wrap. </param>
    public DefaultRestResponseHeaders(IDictionary<string, string> headers)
    {
        Guard.IsNotNull(headers);

        _headers = headers;
    }

    /// <summary>
    ///     Gets a header with the specified name.
    ///     If the header is not present the returned optional will not have a value.
    /// </summary>
    /// <param name="name"> The name of the header. </param>
    /// <returns>
    ///     An optional representing the header's presence and value.
    /// </returns>
    public Optional<string> GetHeader(string name)
    {
        if (_headers.TryGetValue(name, out var value))
            return value;

        return default;
    }

    /// <summary>
    ///     Gets a header with the specified name and converts its value via the given converter.
    ///     If the header is not present the returned optional will not have a value.
    /// </summary>
    /// <param name="name"> The name of the header. </param>
    /// <param name="converter"> The converter for the header value. </param>
    /// <returns>
    ///     An optional representing the header's presence and converted value.
    /// </returns>
    public Optional<T> GetHeader<T>(string name, Converter<string, T> converter)
    {
        return Optional.Convert(GetHeader(name), converter);
    }

    private static double ParseDouble(string value)
    {
        return double.Parse(value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
    }
}

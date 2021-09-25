﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord.Rest.Api.Default
{
    /// <summary>
    ///     Represents the known response headers for REST requests
    ///     that are used throughout the default components.
    /// </summary>
    public readonly struct DefaultRestResponseHeaders
    {
        /// <summary>
        ///     Gets whether the request hit the global rate-limit.
        ///     Header: <c>X-RateLimit-Global</c>.
        /// </summary>
        public Optional<bool> IsGlobal => GetHeader("X-RateLimit-Global", bool.Parse);

        /// <summary>
        ///     Header: <c>Retry-After</c>.
        /// </summary>
        public Optional<TimeSpan> RetryAfter => GetHeader("Retry-After", x => TimeSpan.FromSeconds(int.Parse(x)));

        /// <summary>
        ///     Header: <c>X-RateLimit-Limit</c>.
        /// </summary>
        public Optional<int> Limit => GetHeader("X-RateLimit-Limit", int.Parse);

        /// <summary>
        ///     Header: <c>X-RateLimit-Remaining</c>.
        /// </summary>
        public Optional<int> Remaining => GetHeader("X-RateLimit-Remaining", int.Parse);

        /// <summary>
        ///     Header: <c>X-RateLimit-Reset</c>.
        /// </summary>
        public Optional<DateTimeOffset> ResetsAt => GetHeader("X-RateLimit-Reset", x => DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(ParseDouble(x)));

        /// <summary>
        ///     Header: <c>X-RateLimit-Reset-After</c>.
        /// </summary>
        public Optional<TimeSpan> ResetsAfter => GetHeader("X-RateLimit-Reset-After", x => TimeSpan.FromSeconds(ParseDouble(x)));

        /// <summary>
        ///     Header: <c>X-RateLimit-Bucket</c>.
        /// </summary>
        public Optional<string> Bucket => GetHeader("X-RateLimit-Bucket");

        /// <summary>
        ///     Header: <c>Date</c>.
        /// </summary>
        public Optional<DateTimeOffset> Date => GetHeader("Date", DateTimeOffset.Parse);

        private readonly IDictionary<string, string> _headers;

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
            => Optional.Convert(GetHeader(name), converter);

        private static double ParseDouble(string value)
            => double.Parse(value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
    }
}

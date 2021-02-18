﻿using System;
using Disqord.Logging;

namespace Disqord.Api
{
    /// <summary>
    ///     Represents a low-level client for the Discord API.
    ///     Do not use this unless you are well aware of how it works.
    /// </summary>
    public interface IApiClient : ILogging, IDisposable
    {
        /// <summary>
        ///     Gets the token used by this API client.
        /// </summary>
        Token Token { get; }
    }
}

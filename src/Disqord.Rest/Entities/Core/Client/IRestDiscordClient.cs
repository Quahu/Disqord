﻿using System;
using Disqord.Logging;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        ILogger Logger { get; }

        RestDownloadable<RestApplication> CurrentApplication { get; }

        RestDownloadable<RestCurrentUser> CurrentUser { get; }

        TokenType TokenType { get; }
    }
}
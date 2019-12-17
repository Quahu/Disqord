using System;
using Disqord.Logging;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        ILogger Logger { get; }

        RestDownloadable<RestApplication> CurrentApplication { get; }

        RestDownloadable<RestCurrentUser> CurrentUser { get; }

        TokenType TokenType { get; }
    }
}
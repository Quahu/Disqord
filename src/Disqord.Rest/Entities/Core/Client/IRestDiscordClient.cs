using System;
using Disqord.Logging;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        ILogger Logger { get; }

        RestFetchable<RestApplication> CurrentApplication { get; }

        RestFetchable<RestCurrentUser> CurrentUser { get; }

        TokenType TokenType { get; }
    }
}
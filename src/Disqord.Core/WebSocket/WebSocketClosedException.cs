using System;

namespace Disqord.WebSocket;

public class WebSocketClosedException : Exception
{
    public int? CloseStatus { get; }

    public string? CloseMessage { get; }

    public WebSocketClosedException(int? closeStatus, string? closeMessage, Exception? exception = null)
        : base($"The web socket was closed ({(closeStatus != null ? $"{closeStatus}: \"{closeMessage}\"" : "no close status")}).", exception)
    {
        CloseStatus = closeStatus;
        CloseMessage = closeMessage;
    }
}
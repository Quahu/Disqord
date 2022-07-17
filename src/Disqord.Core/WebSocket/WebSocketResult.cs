namespace Disqord.WebSocket;

public readonly struct WebSocketResult
{
    public int Count { get; }

    public WebSocketMessageType MessageType { get; }

    public bool EndOfMessage { get; }

    public WebSocketResult(int count, WebSocketMessageType messageType, bool endOfMessage)
    {
        Count = count;
        MessageType = messageType;
        EndOfMessage = endOfMessage;
    }
}
using System;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IMessageChannel : IChannel, IMessagable
    {
        Snowflake? LastMessageId { get; }

        DateTimeOffset? LastPinTimestamp { get; }

        Task TriggerTypingAsync(RestRequestOptions options = null);

        Task MarkAsReadAsync(RestRequestOptions options = null);

        IDisposable Typing();
    }
}

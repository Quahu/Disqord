using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IDiscordClient : IDisposable
    {
        Task MarkMessageAsReadAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);
    }
}

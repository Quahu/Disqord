using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task MarkMessageAsReadAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);
    }
}

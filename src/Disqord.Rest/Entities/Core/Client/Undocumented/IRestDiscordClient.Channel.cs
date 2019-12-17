using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task MarkMessageAsReadAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);
    }
}

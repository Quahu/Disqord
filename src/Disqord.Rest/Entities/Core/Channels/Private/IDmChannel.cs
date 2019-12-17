using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public interface IDmChannel : IPrivateChannel
    {
        IUser Recipient { get; }

        Task CloseAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id, options);
    }
}

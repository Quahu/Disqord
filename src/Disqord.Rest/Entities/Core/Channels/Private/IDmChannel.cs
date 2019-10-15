using System.Threading.Tasks;

namespace Disqord
{
    public interface IDmChannel : IPrivateChannel
    {
        IUser Recipient { get; }

        Task CloseAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id, options);
    }
}

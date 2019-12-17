using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IGroupChannel : IPrivateChannel, IDeletable
    {
        Task LeaveAsync(RestRequestOptions options = null);

        Task IDeletable.DeleteAsync(RestRequestOptions options)
            => LeaveAsync(options);
    }
}

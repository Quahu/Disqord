using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IUser : IMessagable, IMentionable, ITaggable
    {
        Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null);
    }
}

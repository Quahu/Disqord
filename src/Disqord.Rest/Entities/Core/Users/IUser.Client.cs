using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IUser : IMessagable, IMentionable, ITaggable
    {
        Task AddFriendAsync(RestRequestOptions options = null);

        Task BlockAsync(RestRequestOptions options = null);

        Task UnfriendOrUnblockAsync(RestRequestOptions options = null);

        Task<RestUserProfile> GetProfileAsync(RestRequestOptions options = null);

        Task SetNoteAsync(string note, RestRequestOptions options = null);

        Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null);
    }
}

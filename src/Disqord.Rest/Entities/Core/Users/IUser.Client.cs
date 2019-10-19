using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IUser : IMessagable, IMentionable, ITaggable
    {
        Task CreateRelationshipAsync(RelationshipType? type = null, RestRequestOptions options = null);

        Task DeleteRelationshipAsync(RestRequestOptions options = null);

        Task SendFriendRequestAsync(RestRequestOptions options = null);

        Task<RestUserProfile> GetProfileAsync(RestRequestOptions options = null);

        Task SetNoteAsync(string note, RestRequestOptions options = null);

        Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null);
    }
}

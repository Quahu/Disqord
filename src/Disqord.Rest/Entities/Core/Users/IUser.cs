using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public interface IUser : IMessagable, IMentionable, ITaggable
    {
        string Name { get; }

        string Discriminator { get; }

        string AvatarHash { get; }

        bool IsBot { get; }

        string GetAvatarUrl(ImageFormat? imageFormat = null, int size = 2048);

        Task SetNoteAsync(string note, RestRequestOptions options = null);

        //Task SendFriendRequestAsync(RestRequestOptions options = null);

        Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null);
    }
}

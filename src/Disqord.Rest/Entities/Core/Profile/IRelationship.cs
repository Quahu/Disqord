using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public interface IRelationship : ISnowflakeEntity, IDeletable
    {
        IUser User { get; }

        RelationshipType Type { get; }

        Task AcceptAsync(RestRequestOptions options = null);
    }
}

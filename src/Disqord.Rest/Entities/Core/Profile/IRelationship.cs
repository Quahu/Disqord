using System.Threading.Tasks;

namespace Disqord
{
    public interface IRelationship : ISnowflakeEntity, IDeletable
    {
        IUser User { get; }

        RelationshipType Type { get; }

        Task AcceptAsync(RestRequestOptions options = null);
    }
}

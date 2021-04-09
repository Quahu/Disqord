using Disqord.Models;

namespace Disqord
{
    public interface IConnection : INamable, IJsonUpdatable<ConnectionJsonModel>
    {
        string Id { get; }

        string Type { get; }

        bool IsRevoked { get; }

        // TODO?
    }
}

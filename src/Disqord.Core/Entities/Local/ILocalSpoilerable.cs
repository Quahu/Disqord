using Qommon;

namespace Disqord;

public interface ILocalSpoilerable
{
    Optional<bool> IsSpoiler { get; set; }
}

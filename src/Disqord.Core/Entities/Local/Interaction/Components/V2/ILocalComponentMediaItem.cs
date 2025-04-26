using Qommon;

namespace Disqord;

public interface ILocalComponentMediaItem : ILocalSpoilerable
{
    Optional<LocalUnfurledMediaItem> Media { get; set; }

    Optional<string?> Description { get; set; }
}

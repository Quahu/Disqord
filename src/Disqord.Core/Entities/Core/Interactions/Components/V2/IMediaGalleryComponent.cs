using System.Collections.Generic;

namespace Disqord;

public interface IMediaGalleryComponent : IComponent
{
    IReadOnlyList<IMediaGalleryItem> Items { get; }
}

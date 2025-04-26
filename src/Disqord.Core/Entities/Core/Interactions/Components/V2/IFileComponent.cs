namespace Disqord;

public interface IFileComponent : IComponent, ISpoilerableEntity
{
    IUnfurledMediaItem File { get; }
}

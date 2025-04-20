namespace Disqord;

public interface IComponentMediaItem : ISpoilerableEntity
{
    IUnfurledMediaItem Media { get; }

    string? Description { get; }
}

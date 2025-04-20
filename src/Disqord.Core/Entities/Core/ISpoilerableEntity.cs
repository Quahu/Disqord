namespace Disqord;

public interface ISpoilerableEntity : IEntity
{
    bool IsSpoiler { get; }
}

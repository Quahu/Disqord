namespace Disqord;

public interface ISeparatorComponent : IComponent
{
    bool IsDivider { get; }

    SeparatorComponentSpacingSize SpacingSize { get; }
}

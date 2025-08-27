namespace Disqord;

public interface ILabelComponent : IComponent
{
    string Label { get; }

    string? Description { get; }

    IComponent Component { get; }
}

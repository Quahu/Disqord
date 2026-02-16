namespace Disqord;

public interface IModalRadioGroupComponent : IModalComponent, ICustomIdentifiableEntity
{
    string? Value { get; }
}

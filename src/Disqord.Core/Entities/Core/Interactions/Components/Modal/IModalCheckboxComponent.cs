namespace Disqord;

public interface IModalCheckboxComponent : IModalComponent, ICustomIdentifiableEntity
{
    bool Value { get; }
}

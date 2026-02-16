using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalCheckboxComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalCheckboxComponent>
{
    public Optional<string> CustomId { get; set; }

    public Optional<bool> IsDefault { get; set; }

    public new Optional<string> Label { get; set; }

    public Optional<bool> IsDisabled { get; set; }

    public LocalCheckboxComponent()
    { }

    protected LocalCheckboxComponent(LocalCheckboxComponent other) : base(other)
    {
        CustomId = other.CustomId;
        IsDefault = other.IsDefault;
        Label = other.Label;
        IsDisabled = other.IsDisabled;
    }

    public override LocalCheckboxComponent Clone()
    {
        return new(this);
    }
}

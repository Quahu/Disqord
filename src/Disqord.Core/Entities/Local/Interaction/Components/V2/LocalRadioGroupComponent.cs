using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalRadioGroupComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalRadioGroupComponent>
{
    public Optional<string> CustomId { get; set; }

    public Optional<IList<LocalRadioGroupOption>> Options { get; set; }

    public Optional<bool> IsRequired { get; set; }

    public Optional<bool> IsDisabled { get; set; }

    public LocalRadioGroupComponent()
    { }

    protected LocalRadioGroupComponent(LocalRadioGroupComponent other) : base(other)
    {
        CustomId = other.CustomId;
        Options = other.Options.DeepClone();
        IsRequired = other.IsRequired;
        IsDisabled = other.IsDisabled;
    }

    public override LocalRadioGroupComponent Clone()
    {
        return new(this);
    }
}

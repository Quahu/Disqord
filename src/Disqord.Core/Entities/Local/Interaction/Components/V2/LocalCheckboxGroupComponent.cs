using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalCheckboxGroupComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalCheckboxGroupComponent>
{
    public Optional<string> CustomId { get; set; }

    public Optional<IList<LocalCheckboxGroupOption>> Options { get; set; }

    public Optional<int> MinimumSelectedOptions { get; set; }

    public Optional<int> MaximumSelectedOptions { get; set; }

    public Optional<bool> IsRequired { get; set; }

    public Optional<bool> IsDisabled { get; set; }

    public LocalCheckboxGroupComponent()
    { }

    protected LocalCheckboxGroupComponent(LocalCheckboxGroupComponent other) : base(other)
    {
        CustomId = other.CustomId;
        Options = other.Options.DeepClone();
        MinimumSelectedOptions = other.MinimumSelectedOptions;
        MaximumSelectedOptions = other.MaximumSelectedOptions;
        IsRequired = other.IsRequired;
        IsDisabled = other.IsDisabled;
    }

    public override LocalCheckboxGroupComponent Clone()
    {
        return new(this);
    }
}

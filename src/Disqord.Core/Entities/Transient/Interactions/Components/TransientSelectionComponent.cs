using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="ISelectionComponent"/>
public class TransientSelectionComponent(ComponentJsonModel model)
    : TransientComponent(model), ISelectionComponent
{
    /// <inheritdoc/>
    public string CustomId => Model.CustomId.Value;

    /// <inheritdoc/>
    public new SelectionComponentType Type => (SelectionComponentType) Model.Type;

    /// <inheritdoc/>
    public IReadOnlyList<ChannelType> ChannelTypes
    {
        get
        {
            if (!Model.ChannelTypes.HasValue)
                return ReadOnlyList<ChannelType>.Empty;

            return Model.ChannelTypes.Value;
        }
    }

    /// <inheritdoc/>
    public string? Placeholder => Model.Placeholder.GetValueOrDefault();

    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyList<IDefaultSelectionValue> DefaultValues => field ??= Model.DefaultValues.Value.ToReadOnlyList(static model => new TransientDefaultSelectionValue(model));

    /// <inheritdoc/>
    public int MinimumSelectedOptions => Model.MinValues.Value;

    /// <inheritdoc/>
    public int MaximumSelectedOptions => Model.MaxValues.Value;

    /// <inheritdoc/>
    public IReadOnlyList<ISelectionComponentOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(static model => new TransientSelectionComponentOption(model));

    private IReadOnlyList<ISelectionComponentOption>? _options;

    /// <inheritdoc/>
    public bool IsDisabled => Model.Disabled.GetValueOrDefault();

    /// <inheritdoc/>
    public bool IsRequired => Model.Required.GetValueOrDefault();
}

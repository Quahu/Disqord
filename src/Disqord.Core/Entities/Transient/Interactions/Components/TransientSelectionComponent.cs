﻿using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="ISelectionComponent"/>
public class TransientSelectionComponent : TransientComponent, ISelectionComponent
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
    public IReadOnlyList<IDefaultSelectionValue> DefaultValues => _defaultValues ??= Model.DefaultValues.Value.ToReadOnlyList(Client, (model, client) => new TransientDefaultSelectionValue(client, model));

    private IReadOnlyList<IDefaultSelectionValue>? _defaultValues;

    /// <inheritdoc/>
    public int MinimumSelectedOptions => Model.MinValues.Value;

    /// <inheritdoc/>
    public int MaximumSelectedOptions => Model.MaxValues.Value;

    /// <inheritdoc/>
    public IReadOnlyList<ISelectionComponentOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(Client, (model, client) => new TransientSelectionComponentOption(client, model));

    private IReadOnlyList<ISelectionComponentOption>? _options;

    /// <inheritdoc/>
    public bool IsDisabled => Model.Disabled.GetValueOrDefault();

    public TransientSelectionComponent(IClient client, ComponentJsonModel model)
        : base(client, model)
    { }
}

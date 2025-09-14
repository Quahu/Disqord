using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IRowComponent"/>
public class TransientRowComponent(ComponentJsonModel model)
    : TransientComponent(model), IRowComponent
{
    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyList<IComponent> Components => field ??= Model.Components.Value.ToReadOnlyList(Create);

    public IEnumerator<IComponent> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

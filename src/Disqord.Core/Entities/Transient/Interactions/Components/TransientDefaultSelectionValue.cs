using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IDefaultSelectionValue"/>
public class TransientDefaultSelectionValue(DefaultValueJsonModel model)
    : TransientEntity<DefaultValueJsonModel>(model), IDefaultSelectionValue
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public DefaultSelectionValueType Type => Model.Type;
}

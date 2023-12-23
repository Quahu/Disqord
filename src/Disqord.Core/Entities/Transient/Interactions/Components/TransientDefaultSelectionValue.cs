using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IDefaultSelectionValue"/>
public class TransientDefaultSelectionValue : TransientClientEntity<DefaultValueJsonModel>, IDefaultSelectionValue
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public DefaultSelectionValueType Type => Model.Type;

    public TransientDefaultSelectionValue(IClient client, DefaultValueJsonModel model)
        : base(client, model)
    { }
}

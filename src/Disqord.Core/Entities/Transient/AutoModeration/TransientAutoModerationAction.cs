using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IAutoModerationAction"/>
public class TransientAutoModerationAction : TransientEntity<AutoModerationActionJsonModel>, IAutoModerationAction
{
    /// <inheritdoc/>
    public AutoModerationActionType Type => Model.Type;

    /// <inheritdoc/>
    public IAutoModerationActionMetadata? Metadata
    {
        get
        {
            if (!Model.Metadata.HasValue)
                return null;

            return _metadata ??= new TransientAutoModerationActionMetadata(Model.Metadata.Value);
        }
    }
    private IAutoModerationActionMetadata? _metadata;

    public TransientAutoModerationAction(AutoModerationActionJsonModel model)
        : base(model)
    { }
}

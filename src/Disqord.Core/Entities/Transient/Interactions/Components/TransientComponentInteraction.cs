using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientComponentInteraction : TransientUserInteraction, IComponentInteraction
{
    /// <inheritdoc/>
    public string CustomId => Model.Data.Value.CustomId.Value;

    /// <inheritdoc/>
    public ComponentType ComponentType => Model.Data.Value.ComponentType.Value;

    /// <inheritdoc/>
    public IReadOnlyList<string> SelectedValues
    {
        get
        {
            if (!Model.Data.Value.Values.TryGetValue(out var values))
                return ReadOnlyList<string>.Empty;

            return values;
        }
    }

    /// <inheritdoc/>
    public IUserMessage Message => _message ??= new TransientUserMessage(Client, Model.Message.Value);

    private IUserMessage? _message;

    public TransientComponentInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}

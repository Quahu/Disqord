using System.Diagnostics.CodeAnalysis;
using Disqord.Rest.Api.Models;

namespace Disqord.Rest;

public class TransientInteractionCallbackResponse(IClient client, InteractionCallbackResponseJsonModel model)
    : TransientEntity<InteractionCallbackResponseJsonModel>(model), IInteractionCallbackResponse
{
    /// <inheritdoc/>
    [field: MaybeNull]
    public ICallbackInteraction Interaction => field ??= new TransientCallbackInteraction(Model.Interaction);

    /// <inheritdoc/>
    public ICallbackResource? Resource => field ??= Model.Resource != null ? new TransientCallbackResource(client, Model.Resource) : null;
}

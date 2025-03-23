using Disqord.Rest.Api.Models;

namespace Disqord.Rest;

public class TransientCallbackResource(IClient client, InteractionCallbackResourceJsonModel model)
    : TransientEntity<InteractionCallbackResourceJsonModel>(model), ICallbackResource
{
    public InteractionResponseType Type => Model.Type;

    public IUserMessage? Message => field ??= Model.Message != null ? new TransientUserMessage(client, Model.Message) : null;
}

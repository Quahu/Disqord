using Disqord.Rest.Api.Models;

namespace Disqord.Rest;

public class TransientCallbackInteraction(CallbackInteractionJsonModel model)
    : TransientEntity<CallbackInteractionJsonModel>(model), ICallbackInteraction
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public InteractionType Type => Model.Type;

    /// <inheritdoc/>
    public Snowflake? ResponseMessageId => Model.ResponseMessageId;

    /// <inheritdoc/>
    public bool? IsResponseMessageLoading => Model.ResponseMessageLoading;

    /// <inheritdoc/>
    public bool? IsResponseMessageEphemeral => Model.ResponseMessageEphemeral;
}

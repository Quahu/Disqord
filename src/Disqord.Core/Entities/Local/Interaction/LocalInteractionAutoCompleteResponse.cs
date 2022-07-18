using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalInteractionAutoCompleteResponse : ILocalInteractionResponse, ILocalConstruct<LocalInteractionAutoCompleteResponse>
{
    InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.ApplicationCommandAutoComplete;

    /// <summary>
    ///     Gets or sets the choices of this response.
    /// </summary>
    public Optional<IList<KeyValuePair<string, object>>> Choices { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionAutoCompleteResponse"/>.
    /// </summary>
    public LocalInteractionAutoCompleteResponse()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionAutoCompleteResponse"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalInteractionAutoCompleteResponse(LocalInteractionAutoCompleteResponse other)
    {
        Choices = other.Choices.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalInteractionAutoCompleteResponse Clone()
    {
        return new(this);
    }
}

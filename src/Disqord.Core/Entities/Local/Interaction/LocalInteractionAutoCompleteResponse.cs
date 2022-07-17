using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord;

public class LocalInteractionAutoCompleteResponse : ILocalInteractionResponse, ILocalConstruct<LocalInteractionAutoCompleteResponse>
{
    InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.ApplicationCommandAutoComplete;

    /// <summary>
    ///     Gets or sets the choices of this response.
    /// </summary>
    public Optional<IList<KeyValuePair<string, object>>> Choices { get; set; }

    public LocalInteractionAutoCompleteResponse()
    { }

    protected LocalInteractionAutoCompleteResponse(LocalInteractionAutoCompleteResponse other)
    {
        Choices = Optional.Convert(other.Choices, choices => choices.ToList() as IList<KeyValuePair<string, object>>);
    }

    /// <inheritdoc/>
    public virtual LocalInteractionAutoCompleteResponse Clone()
    {
        return new(this);
    }
}

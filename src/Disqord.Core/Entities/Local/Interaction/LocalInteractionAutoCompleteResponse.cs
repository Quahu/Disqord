using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord;

public class LocalInteractionAutoCompleteResponse : ILocalInteractionResponse
{
    public InteractionResponseType Type { get; set; }

    /// <summary>
    ///     Gets or sets the choices of this response.
    /// </summary>
    public IList<LocalSlashCommandOptionChoice> Choices { get; set; }

    public LocalInteractionAutoCompleteResponse()
    {
        Type = InteractionResponseType.ApplicationCommandAutoComplete;
        Choices = new List<LocalSlashCommandOptionChoice>();
    }

    private LocalInteractionAutoCompleteResponse(LocalInteractionAutoCompleteResponse other)
    {
        Type = other.Type;
        Choices = other.Choices.Select(x => x.Clone()).ToList();
    }

    public virtual LocalInteractionAutoCompleteResponse Clone()
        => new(this);

    object ICloneable.Clone()
        => Clone();

    public void Validate()
    {
        if (Type != InteractionResponseType.ApplicationCommandAutoComplete)
            throw new InvalidOperationException("The interaction response's type must be autocomplete.");
    }
}

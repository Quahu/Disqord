using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public class LocalInteractionAutoCompleteResponse : ILocalInteractionResponse
    {
        InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.ApplicationCommandAutoComplete;

        /// <summary>
        ///     Gets or sets the choices of this response.
        /// </summary>
        public Optional<IList<LocalSlashCommandOptionChoice>> Choices { get; set; }

        public LocalInteractionAutoCompleteResponse()
        { }

        protected LocalInteractionAutoCompleteResponse(LocalInteractionAutoCompleteResponse other)
        {
            Choices = Optional.Convert(other.Choices, choices => choices?.Select(choice => choice?.Clone()).ToList() as IList<LocalSlashCommandOptionChoice>);
        }

        public virtual LocalInteractionAutoCompleteResponse Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}

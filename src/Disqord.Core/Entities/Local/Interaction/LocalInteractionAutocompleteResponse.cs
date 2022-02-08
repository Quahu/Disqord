using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalInteractionAutocompleteResponse : ILocalInteractionResponse
    {
        public InteractionResponseType Type { get; set; }

        /// <summary>
        ///     Gets or sets the choices of this response.
        /// </summary>
        public IList<LocalSlashCommandOptionChoice> Choices { get; set; }

        public LocalInteractionAutocompleteResponse()
        {
            Type = InteractionResponseType.ApplicationCommandAutocomplete;
            Choices = new List<LocalSlashCommandOptionChoice>();
        }

        private LocalInteractionAutocompleteResponse(LocalInteractionAutocompleteResponse other)
        {
            Type = other.Type;
            Choices = other.Choices.Select(x => x.Clone()).ToList();
        }

        public virtual LocalInteractionAutocompleteResponse Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}

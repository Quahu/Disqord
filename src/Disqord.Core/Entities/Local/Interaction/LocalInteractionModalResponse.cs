using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalInteractionModalResponse : ILocalInteractionResponse
    {
        InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.Modal;

        /// <summary>
        ///     Gets or sets the custom ID of this modal.
        /// </summary>
        public string CustomId { get; set; }

        /// <summary>
        ///     Gets or sets the title of this modal.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the components of this modal.
        /// </summary>
        public IList<LocalComponent> Components { get; set; }

        public LocalInteractionModalResponse()
        {
            Components = new List<LocalComponent>();
        }

        protected LocalInteractionModalResponse(LocalInteractionModalResponse other)
        {
            CustomId = other.CustomId;
            Title = other.Title;
            Components = other.Components.Select(component => component?.Clone()).ToList();
        }

        public virtual LocalInteractionModalResponse Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}

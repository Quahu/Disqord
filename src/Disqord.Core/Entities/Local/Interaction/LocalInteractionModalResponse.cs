using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public class LocalInteractionModalResponse : ILocalInteractionResponse, ILocalCustomIdentifiableEntity
    {
        InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.Modal;

        /// <summary>
        ///     Gets or sets the custom ID of this modal.
        /// </summary>
        public Optional<string> CustomId { get; set; }

        /// <summary>
        ///     Gets or sets the title of this modal.
        /// </summary>
        public Optional<string> Title { get; set; }

        /// <summary>
        ///     Gets or sets the components of this modal.
        /// </summary>
        public Optional<IList<LocalComponent>> Components { get; set; }

        public LocalInteractionModalResponse()
        { }

        protected LocalInteractionModalResponse(LocalInteractionModalResponse other)
        {
            CustomId = other.CustomId;
            Title = other.Title;
            Components = Optional.Convert(other.Components, components => components?.Select(component => component?.Clone()).ToList() as IList<LocalComponent>);
        }

        public virtual LocalInteractionModalResponse Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}

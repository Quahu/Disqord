using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalInteractionModalResponse : ILocalInteractionResponse
    {
        public const int MaxCustomIdLength = 100;

        public const int MaxTitleLength = 100; //TODO: Check this

        InteractionResponseType ILocalInteractionResponse.Type => InteractionResponseType.Modal;

        /// <summary>
        ///     Gets or sets the custom ID of this modal.
        /// </summary>
        public string CustomId
        {
            get => _customId;
            set
            {
                if (value != null && value.Length > MaxCustomIdLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The modal's custom ID must not be longer than {MaxCustomIdLength} characters.");

                _customId = value;
            }
        }
        private string _customId;

        /// <summary>
        ///     Gets or sets the title of this modal.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (value != null && value.Length > MaxTitleLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The modal's title must not be longer than {MaxTitleLength} characters.");

                _title = value;
            }
        }
        private string _title;

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

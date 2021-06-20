using System;

namespace Disqord
{
    public class LocalInteractionResponse : LocalMessageBase
    {
        public InteractionResponseType Type { get; set; }

        public InteractionResponseFlag Flags { get; set; }

        public bool IsEphemeral
        {
            get => Flags.HasFlag(InteractionResponseFlag.Ephemeral);
            set
            {
                if (value)
                    Flags |= InteractionResponseFlag.Ephemeral;
                else
                    Flags &= ~InteractionResponseFlag.Ephemeral;
            }
        }

        public LocalInteractionResponse()
        { }

        public LocalInteractionResponse(InteractionResponseType type)
        {
            Type = type;
        }

        private LocalInteractionResponse(LocalInteractionResponse other)
            : base(other)
        {
            Type = other.Type;
            Flags = other.Flags;
        }

        public LocalInteractionResponse WithIsEphemeral(bool isEphemeral = true)
        {
            IsEphemeral = isEphemeral;
            return this;
        }

        public override LocalInteractionResponse Clone()
            => new(this);

        public override void Validate()
        {
            if (Type == default)
                throw new InvalidOperationException("The interaction response's type must be set.");

            base.Validate();
        }
    }
}

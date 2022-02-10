namespace Disqord
{
    public class LocalInteractionMessageResponse : LocalMessageBase, ILocalInteractionResponse
    {
        public InteractionResponseType Type { get; set; }

        public bool IsEphemeral
        {
            get => Flags.HasFlag(MessageFlag.Ephemeral);
            set
            {
                if (value)
                    Flags |= MessageFlag.Ephemeral;
                else
                    Flags &= ~MessageFlag.Ephemeral;
            }
        }

        public LocalInteractionMessageResponse()
        { }

        public LocalInteractionMessageResponse(InteractionResponseType type)
        {
            Type = type;
        }

        private LocalInteractionMessageResponse(LocalInteractionMessageResponse other)
            : base(other)
        {
            Type = other.Type;
            Flags = other.Flags;
        }

        public override LocalInteractionMessageResponse Clone()
            => new(this);

        public override void Validate()
        {
            if (Type == InteractionResponseType.ChannelMessage)
                base.Validate();
        }
    }
}

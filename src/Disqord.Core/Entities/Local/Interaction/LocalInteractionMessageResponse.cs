namespace Disqord
{
    public class LocalInteractionMessageResponse : LocalInteractionFollowup, ILocalInteractionResponse
    {
        public InteractionResponseType Type { get; set; }

        public LocalInteractionMessageResponse()
        { }

        public LocalInteractionMessageResponse(InteractionResponseType type)
        {
            Type = type;
        }

        protected LocalInteractionMessageResponse(LocalInteractionMessageResponse other)
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

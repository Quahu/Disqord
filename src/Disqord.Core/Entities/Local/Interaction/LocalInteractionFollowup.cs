namespace Disqord
{
    public class LocalInteractionFollowup : LocalMessageBase
    {
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

        public LocalInteractionFollowup()
        { }

        protected LocalInteractionFollowup(LocalInteractionFollowup other)
            : base(other)
        {
            Flags = other.Flags;
        }

        public override LocalInteractionFollowup Clone()
            => new(this);
    }
}

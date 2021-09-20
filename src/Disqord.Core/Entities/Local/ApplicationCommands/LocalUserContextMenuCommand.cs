namespace Disqord
{
    public class LocalUserContextMenuCommand : LocalApplicationCommand
    {
        public LocalUserContextMenuCommand(string name)
            : base(name)
        { }

        protected LocalUserContextMenuCommand(LocalApplicationCommand other)
            : base(other)
        { }

        public override LocalUserContextMenuCommand Clone()
            => new(this);
    }
}

namespace Disqord
{
    public class LocalUserContextMenuCommand : LocalContextMenuCommand
    {
        public LocalUserContextMenuCommand(string name)
            : base(name)
        { }

        protected LocalUserContextMenuCommand(LocalUserContextMenuCommand other)
            : base(other)
        { }

        public override LocalUserContextMenuCommand Clone()
            => new(this);
    }
}

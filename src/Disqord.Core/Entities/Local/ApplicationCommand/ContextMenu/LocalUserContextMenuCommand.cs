namespace Disqord
{
    public class LocalUserContextMenuCommand : LocalContextMenuCommand
    {
        public LocalUserContextMenuCommand()
        { }

        protected LocalUserContextMenuCommand(LocalUserContextMenuCommand other)
            : base(other)
        { }

        public override LocalUserContextMenuCommand Clone()
            => new(this);
    }
}

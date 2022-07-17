namespace Disqord;

public class LocalMessageContextMenuCommand : LocalContextMenuCommand
{
    public LocalMessageContextMenuCommand()
    { }

    protected LocalMessageContextMenuCommand(LocalMessageContextMenuCommand other)
        : base(other)
    { }

    public override LocalMessageContextMenuCommand Clone()
        => new(this);
}
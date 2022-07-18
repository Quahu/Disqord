namespace Disqord;

public class LocalUserContextMenuCommand : LocalContextMenuCommand, ILocalConstruct<LocalUserContextMenuCommand>
{
    /// <summary>
    ///     Instantiates a new <see cref="LocalUserContextMenuCommand"/>.
    /// </summary>
    public LocalUserContextMenuCommand()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalUserContextMenuCommand"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalUserContextMenuCommand(LocalUserContextMenuCommand other)
        : base(other)
    { }

    /// <inheritdoc/>
    public override LocalUserContextMenuCommand Clone()
    {
        return new(this);
    }
}

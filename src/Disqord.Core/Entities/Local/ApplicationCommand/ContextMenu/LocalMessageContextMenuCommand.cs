namespace Disqord;

public class LocalMessageContextMenuCommand : LocalContextMenuCommand, ILocalConstruct<LocalMessageContextMenuCommand>
{
    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageContextMenuCommand"/>.
    /// </summary>
    public LocalMessageContextMenuCommand()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageContextMenuCommand"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMessageContextMenuCommand(LocalMessageContextMenuCommand other)
        : base(other)
    { }

    /// <inheritdoc/>
    public override LocalMessageContextMenuCommand Clone()
    {
        return new(this);
    }
}

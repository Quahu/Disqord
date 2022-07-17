using Qommon;

namespace Disqord;

public class LocalLinkButtonComponent : LocalButtonComponentBase, ILocalConstruct<LocalLinkButtonComponent>
{
    /// <summary>
    ///     Gets or sets the URL of this link button component.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Url { get; set; }

    public LocalLinkButtonComponent()
    { }

    protected LocalLinkButtonComponent(LocalLinkButtonComponent other)
        : base(other)
    {
        Url = other.Url;
    }

    public override LocalLinkButtonComponent Clone()
    {
        return new(this);
    }
}

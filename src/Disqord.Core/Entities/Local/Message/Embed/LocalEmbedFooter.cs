using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local embed footer.
/// </summary>
public class LocalEmbedFooter : ILocalConstruct<LocalEmbedFooter>, IJsonConvertible<EmbedFooterJsonModel>
{
    /// <summary>
    ///     Gets or sets the text of this footer.
    /// </summary>
    public Optional<string> Text { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the icon of this footer.
    /// </summary>
    public Optional<string> IconUrl { get; set; }

    /// <summary>
    ///     Gets the total text length of this footer.
    /// </summary>
    public int Length => Text.GetValueOrDefault()?.Length ?? 0;

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedFooter"/>.
    /// </summary>
    public LocalEmbedFooter()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedFooter"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalEmbedFooter(LocalEmbedFooter other)
    {
        Text = other.Text;
        IconUrl = other.IconUrl;
    }

    /// <inheritdoc/>
    public virtual LocalEmbedFooter Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public virtual EmbedFooterJsonModel ToModel()
    {
        OptionalGuard.HasValue(Text);

        return new EmbedFooterJsonModel
        {
            Text = Text.Value,
            IconUrl = IconUrl
        };
    }
}

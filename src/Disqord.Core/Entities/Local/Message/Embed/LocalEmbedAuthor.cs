using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local embed author.
/// </summary>
public class LocalEmbedAuthor : ILocalConstruct<LocalEmbedAuthor>, IJsonConvertible<EmbedAuthorJsonModel>
{
    /// <summary>
    ///     Gets or sets the name of this author.
    /// </summary>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets the URL of this author.
    /// </summary>
    public Optional<string> Url { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the icon of this author.
    /// </summary>
    public Optional<string> IconUrl { get; set; }

    /// <summary>
    ///     Gets the total text length of this author.
    /// </summary>
    public int Length => Name.GetValueOrDefault()?.Length ?? 0;

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedAuthor"/>.
    /// </summary>
    public LocalEmbedAuthor()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedAuthor"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalEmbedAuthor(LocalEmbedAuthor other)
    {
        Name = other.Name;
        Url = other.Url;
        IconUrl = other.IconUrl;
    }

    /// <inheritdoc/>
    public virtual LocalEmbedAuthor Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual EmbedAuthorJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);

        return new EmbedAuthorJsonModel
        {
            Name = Name.Value,
            Url = Url,
            IconUrl = IconUrl
        };
    }

    /// <summary>
    ///     Converts the specified embed author to a <see cref="LocalEmbedAuthor"/>.
    /// </summary>
    /// <param name="author"> The embed author to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalEmbedAuthor"/>.
    /// </returns>
    public static LocalEmbedAuthor CreateFrom(IEmbedAuthor author)
    {
        return new LocalEmbedAuthor
        {
            Name = author.Name,
            Url = Optional.FromNullable(author.Url),
            IconUrl = Optional.FromNullable(author.IconUrl)
        };
    }
}

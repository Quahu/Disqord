using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalEmbed : ILocalConstruct<LocalEmbed>, IJsonConvertible<EmbedJsonModel>
{
    /// <summary>
    ///     Gets or sets the title of this embed.
    /// </summary>
    public Optional<string> Title { get; set; }

    /// <summary>
    ///     Gets or sets the description of this embed.
    /// </summary>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the URL of this embed.
    /// </summary>
    public Optional<string> Url { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the image of this embed.
    /// </summary>
    public Optional<string> ImageUrl { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the thumbnail of this embed.
    /// </summary>
    public Optional<string> ThumbnailUrl { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of this embed.
    /// </summary>
    public Optional<DateTimeOffset> Timestamp { get; set; }

    /// <summary>
    ///     Gets or sets the color of this embed.
    /// </summary>
    public Optional<Color> Color { get; set; }

    /// <summary>
    ///     Gets or sets the footer of this embed.
    /// </summary>
    public Optional<LocalEmbedFooter> Footer { get; set; }

    /// <summary>
    ///     Gets or sets the author of this embed.
    /// </summary>
    public Optional<LocalEmbedAuthor> Author { get; set; }

    /// <summary>
    ///     Gets or sets the fields of this embed.
    /// </summary>
    public Optional<IList<LocalEmbedField>> Fields { get; set; }

    /// <summary>
    ///     Gets the total text length of this embed.
    /// </summary>
    public int Length
    {
        get
        {
            var titleLength = Title.GetValueOrDefault()?.Length ?? 0;
            var descriptionLength = Description.GetValueOrDefault()?.Length ?? 0;
            var footerLength = Footer.GetValueOrDefault()?.Length ?? 0;
            var authorLength = Author.GetValueOrDefault()?.Length ?? 0;
            var fieldsLength = Fields.GetValueOrDefault()?.Sum(field => field.Length) ?? 0;
            return titleLength + descriptionLength + footerLength + authorLength + fieldsLength;
        }
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbed"/>.
    /// </summary>
    public LocalEmbed()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbed"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalEmbed(LocalEmbed other)
    {
        Title = other.Title;
        Description = other.Description;
        Url = other.Url;
        ImageUrl = other.ImageUrl;
        ThumbnailUrl = other.ThumbnailUrl;
        Timestamp = other.Timestamp;
        Color = other.Color;
        Footer = other.Footer.Clone();
        Author = other.Author.Clone();
        Fields = other.Fields.DeepClone();
    }

    /// <inheritdoc/>
    public virtual LocalEmbed Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual EmbedJsonModel ToModel()
    {
        return new EmbedJsonModel
        {
            Title = Title,
            Type = "rich",
            Description = Description,
            Url = Url,
            Timestamp = Timestamp,
            Color = Optional.Convert(Color, color => color.RawValue),
            Image = Optional.Convert(ImageUrl, imageUrl => new EmbedImageJsonModel
            {
                Url = imageUrl
            }),
            Thumbnail = Optional.Convert(ThumbnailUrl, thumbnailUrl => new EmbedThumbnailJsonModel
            {
                Url = thumbnailUrl
            }),
            Footer = Optional.Convert(Footer, footer => footer.ToModel()),
            Author = Optional.Convert(Author, author => author.ToModel()),
            Fields = Optional.Convert(Fields, fields => fields.Select(field => field.ToModel()).ToArray())
        };
    }

    /// <inheritdoc cref="CreateFrom"/>
    [Obsolete("Use CreateFrom() instead.")]
    public static LocalEmbed FromEmbed(IEmbed embed)
    {
        return CreateFrom(embed);
    }

    /// <summary>
    ///     Converts the specified embed to a <see cref="LocalEmbed"/>.
    /// </summary>
    /// <param name="embed"> The embed to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalEmbed"/>.
    /// </returns>
    public static LocalEmbed CreateFrom(IEmbed embed)
    {
        var builder = new LocalEmbed
        {
            Title = Optional.FromNullable(embed.Title),
            Description = Optional.FromNullable(embed.Description),
            Url = Optional.FromNullable(embed.Url),
            ImageUrl = Optional.FromNullable(embed.Image?.Url),
            ThumbnailUrl = Optional.FromNullable(embed.Thumbnail?.Url),
            Timestamp = Optional.FromNullable(embed.Timestamp),
            Color = Optional.FromNullable(embed.Color)
        };

        if (embed.Footer != null)
            builder.Footer = LocalEmbedFooter.CreateFrom(embed.Footer);

        if (embed.Author != null)
            builder.Author = LocalEmbedAuthor.CreateFrom(embed.Author);

        var fields = embed.Fields;
        var fieldCount = fields.Count;
        for (var i = 0; i < fieldCount; i++)
        {
            var field = fields[i];
            builder.AddField(LocalEmbedField.CreateFrom(field));
        }

        return builder;
    }
}

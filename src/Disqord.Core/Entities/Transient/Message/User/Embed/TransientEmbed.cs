using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <summary>
///     Represents a transient message embed.
/// </summary>
public class TransientEmbed : TransientEntity<EmbedJsonModel>, IEmbed
{
    /// <inheritdoc/>
    public string? Title => Model.Title.GetValueOrDefault();

    /// <inheritdoc/>
    public string? Type => Model.Type.GetValueOrDefault();

    /// <inheritdoc/>
    public string? Description => Model.Description.GetValueOrDefault();

    /// <inheritdoc/>
    public string? Url => Model.Url.GetValueOrDefault();

    /// <inheritdoc/>
    public DateTimeOffset? Timestamp => Model.Timestamp.GetValueOrNullable();

    /// <inheritdoc/>
    public Color? Color => Model.Color.GetValueOrNullable();

    /// <inheritdoc/>
    public IEmbedImage? Image
    {
        get
        {
            if (!Model.Image.HasValue)
                return null;

            return _image ??= new TransientEmbedImage(Model.Image.Value);
        }
    }
    private IEmbedImage? _image;

    /// <inheritdoc/>
    public IEmbedThumbnail? Thumbnail
    {
        get
        {
            if (!Model.Thumbnail.HasValue)
                return null;

            return _thumbnail ??= new TransientEmbedThumbnail(Model.Thumbnail.Value);
        }
    }
    private IEmbedThumbnail? _thumbnail;

    /// <inheritdoc/>
    public IEmbedVideo? Video
    {
        get
        {
            if (!Model.Video.HasValue)
                return null;

            return _video ??= new TransientEmbedVideo(Model.Video.Value);
        }
    }
    private IEmbedVideo? _video;

    /// <inheritdoc/>
    public IEmbedProvider? Provider
    {
        get
        {
            if (!Model.Provider.HasValue)
                return null;

            return _provider ??= new TransientEmbedProvider(Model.Provider.Value);
        }
    }
    private IEmbedProvider? _provider;

    /// <inheritdoc/>
    public IEmbedFooter? Footer
    {
        get
        {
            if (!Model.Footer.HasValue)
                return null;

            return _footer ??= new TransientEmbedFooter(Model.Footer.Value);
        }
    }
    private IEmbedFooter? _footer;

    /// <inheritdoc/>
    public IEmbedAuthor? Author
    {
        get
        {
            if (!Model.Author.HasValue)
                return null;

            return _author ??= new TransientEmbedAuthor(Model.Author.Value);
        }
    }
    private IEmbedAuthor? _author;

    /// <inheritdoc/>
    public IReadOnlyList<IEmbedField> Fields
    {
        get
        {
            if (!Model.Fields.HasValue || Model.Fields.Value.Length == 0)
                return ReadOnlyList<IEmbedField>.Empty;

            return _fields ??= Model.Fields.Value.ToReadOnlyList(model => new TransientEmbedField(model));
        }
    }
    private IReadOnlyList<IEmbedField>? _fields;

    public TransientEmbed(EmbedJsonModel model)
        : base(model)
    { }
}

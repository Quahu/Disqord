using System;
using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a message embed.
/// </summary>
public interface IEmbed : IEntity
{
    /// <summary>
    ///     Gets the tile of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Title { get; }

    /// <summary>
    ///     Gets the type of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Type { get; }

    /// <summary>
    ///     Gets the description of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the URL of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Url { get; }

    /// <summary>
    ///     Gets the timestamp of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    DateTimeOffset? Timestamp { get; }

    /// <summary>
    ///     Gets the color of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    Color? Color { get; }

    /// <summary>
    ///     Gets the image of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedImage? Image { get; }

    /// <summary>
    ///     Gets the thumbnail of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedThumbnail? Thumbnail { get; }

    /// <summary>
    ///     Gets the video of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedVideo? Video { get; }

    /// <summary>
    ///     Gets the provider of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedProvider? Provider { get; }

    /// <summary>
    ///     Gets the footer of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedFooter? Footer { get; }

    /// <summary>
    ///     Gets the author of this embed.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    IEmbedAuthor? Author { get; }

    /// <summary>
    ///     Gets the fields of this embed.
    /// </summary>
    IReadOnlyList<IEmbedField> Fields { get; }
}

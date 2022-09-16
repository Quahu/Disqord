using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local emoji.
/// </summary>
public class LocalEmoji : IEmoji, ILocalConstruct<LocalEmoji>, IJsonConvertible<EmojiJsonModel>
{
    /// <summary>
    ///     Creates a <see cref="LocalEmoji"/> representing a Unicode emoji.
    /// </summary>
    /// <param name="unicodeEmoji"> The Unicode emoji. </param>
    /// <returns>
    ///     A new <see cref="LocalEmoji"/> instance.
    /// </returns>
    public static LocalEmoji Unicode(string unicodeEmoji)
    {
        return new(unicodeEmoji);
    }

    /// <summary>
    ///     Creates a <see cref="LocalCustomEmoji"/> representing a custom emoji.
    /// </summary>
    /// <param name="id"> The ID of the emoji. </param>
    /// <param name="name"> The name of the emoji. </param>
    /// <param name="isAnimated"> Whether the emoji is animated. </param>
    /// <returns>
    ///     A new <see cref="LocalCustomEmoji"/> instance.
    /// </returns>
    public static LocalCustomEmoji Custom(Snowflake id, string? name = null, bool isAnimated = false)
    {
        return new(id, name, isAnimated);
    }

    /// <summary>
    ///     Creates a <see cref="LocalEmoji"/> from the provided <see cref="string"/>.
    /// </summary>
    /// <param name="emojiString"> The <see cref="string"/> containing the emoji. </param>
    /// <returns>
    ///     <see cref="LocalCustomEmoji"/> if the <see cref="string"/> is a valid custom emoji format
    ///     and <see cref="LocalEmoji"/> otherwise.
    /// </returns>
    public static LocalEmoji FromString(string emojiString)
    {
        return LocalCustomEmoji.TryParse(emojiString, out var customEmoji)
            ? customEmoji
            : Unicode(emojiString);
    }

    /// <summary>
    ///     Creates a <see cref="LocalEmoji"/> from the provided <see cref="IEmoji"/>.
    /// </summary>
    /// <param name="emoji"> The <see cref="IEmoji"/>. </param>
    /// <returns>
    ///     <see cref="LocalCustomEmoji"/> if <paramref name="emoji"/> is a custom emoji
    ///     and <see cref="LocalEmoji"/> otherwise or <see langword="null"/> if <paramref name="emoji"/>
    ///     is <see langword="null"/> or has no name.
    /// </returns>
    public static LocalEmoji? FromEmoji(IEmoji? emoji)
    {
        if (emoji is ICustomEmoji customEmoji)
            return new LocalCustomEmoji(customEmoji.Id, customEmoji.Name, customEmoji.IsAnimated);

        return !string.IsNullOrEmpty(emoji?.Name)
            ? new LocalEmoji(emoji.Name)
            : null;
    }

    /// <summary>
    ///     Gets or sets the name of this emoji.
    /// </summary>
    /// <remarks>
    ///     For Unicode emojis (<see cref="LocalEmoji"/>) this is the Unicode emoji
    ///     and is required to be set to a non-null value.<para/>
    ///     For custom emojis (<see cref="LocalCustomEmoji"/>) this is the name
    ///     of the custom emoji and is not required and <i>may</i> be set to <see langword="null"/>.
    /// </remarks>
    public Optional<string?> Name { get; set; }

    string? IPossiblyNamableEntity.Name => Name.GetValueOrDefault();

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmoji"/>.
    /// </summary>
    public LocalEmoji()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmoji"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalEmoji(LocalEmoji other)
    {
        Name = other.Name;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmoji"/> with the specified Unicode emoji.
    /// </summary>
    /// <param name="unicodeEmoji"> The Unicode emoji. </param>
    public LocalEmoji(string unicodeEmoji)
    {
        Name = unicodeEmoji;
    }

    /// <inheritdoc/>
    public virtual LocalEmoji Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual EmojiJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);

        return new EmojiJsonModel
        {
            Name = Name.Value
        };
    }

    /// <inheritdoc/>
    public virtual bool Equals(IEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is IEmoji other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Comparers.Emoji.GetHashCode(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.GetString();
    }
}

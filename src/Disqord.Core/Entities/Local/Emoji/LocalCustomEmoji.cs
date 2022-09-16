using System;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local custom emoji.
/// </summary>
public class LocalCustomEmoji : LocalEmoji, ICustomEmoji, ILocalConstruct<LocalCustomEmoji>
{
    /// <summary>
    ///     Gets or sets the id of this emoji.
    /// </summary>
    public Optional<Snowflake> Id { get; set; }

    /// <summary>
    ///     Gets or sets whether this emoji is animated.
    /// </summary>
    public Optional<bool> IsAnimated { get; set; }

    /// <inheritdoc/>
    public string Tag => this.GetString();

    Snowflake IIdentifiableEntity.Id
    {
        get
        {
            OptionalGuard.HasValue(Id);

            return Id.Value;
        }
    }

    bool ICustomEmoji.IsAnimated => IsAnimated.GetValueOrDefault();

    /// <summary>
    ///     Instantiates a new <see cref="LocalCustomEmoji"/>.
    /// </summary>
    public LocalCustomEmoji()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalCustomEmoji"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalCustomEmoji(LocalCustomEmoji other)
        : base(other)
    {
        Id = other.Id;
        IsAnimated = other.IsAnimated;
    }

    /// <summary>
    ///     Instantiates a new custom emoji with the specified custom emoji ID
    ///     and optionally a name and whether the emoji is animated.
    /// </summary>
    /// <remarks>
    ///     The optional parameters are purely for the developer's convenience and have
    ///     no effect on any Discord API interactions.
    /// </remarks>
    /// <param name="id"> The ID of this emoji. </param>
    /// <param name="name"> The name of this emoji. </param>
    /// <param name="isAnimated"> Whether this emoji is animated. </param>
    public LocalCustomEmoji(Snowflake id, string? name = null, bool isAnimated = false)
        : base(name!)
    {
        Id = id;
        IsAnimated = isAnimated;
    }

    /// <inheritdoc/>
    public override LocalCustomEmoji Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public override EmojiJsonModel ToModel()
    {
        OptionalGuard.HasValue(Id);

        return new EmojiJsonModel
        {
            Id = Id.GetValueOrNullable(),
            Name = Name.GetValueOrDefault(),
            Animated = IsAnimated
        };
    }

    /// <inheritdoc/>
    public bool Equals(ICustomEmoji? other)
    {
        return Comparers.Emoji.Equals(this, other);
    }

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char},out Disqord.LocalCustomEmoji)"/>
    public static bool TryParse(string? value, [MaybeNullWhen(false)] out LocalCustomEmoji result)
    {
        if (value == null)
        {
            result = null;
            return false;
        }

        return TryParse(value.AsSpan(), out result);
    }

    /// <summary>
    ///     Attempts to parse the specified value into a <see cref="LocalCustomEmoji"/>.
    /// </summary>
    /// <remarks>
    ///     If <see langword="true"/> is returned <paramref name="result"/>
    ///     is guaranteed to have the <see cref="Id"/> and <see cref="LocalEmoji.Name"/> properties
    ///     set to valid values.
    /// </remarks>
    /// <param name="value"> The input value. </param>
    /// <param name="result"> The parsed emoji. </param>
    /// <returns>
    ///     <see langword="true"/> if the parse was successful.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> value, [MaybeNullWhen(false)] out LocalCustomEmoji result)
    {
        result = null;
        if (value.Length < 21)
            return false;

        if (value[0] != '<' || value[^1] != '>')
            return false;

        value = value.Slice(1, value.Length - 2);
        var isAnimated = value[0] == 'a';
        if (value[isAnimated ? 1 : 0] != ':')
            return false;

        value = value.Slice(isAnimated ? 2 : 1);
        var colonIndex = value.IndexOf(':');
        if (colonIndex == -1)
            return false;

        var nameSpan = value.Slice(0, colonIndex);
        if (nameSpan.IsEmpty || nameSpan.Length > 32 || nameSpan.IsWhiteSpace())
            return false;

        var idSpan = value.Slice(colonIndex + 1);
        if (!Snowflake.TryParse(idSpan, out var id))
            return false;

        result = new LocalCustomEmoji(id, new string(nameSpan), isAnimated);
        return true;
    }
}

using System;

namespace Disqord;

/// <summary>
///     Represents an emoji.
///     This can be a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) or a default Unicode emoji (e.g. <c>🍿</c>).
/// </summary>
/// <remarks>
///     For Unicode emojis the <see cref="IPossiblyNamableEntity.Name"/> property returns the Unicode instead.
/// </remarks>
public interface IEmoji : IPossiblyNamableEntity, IEquatable<IEmoji>
{ }

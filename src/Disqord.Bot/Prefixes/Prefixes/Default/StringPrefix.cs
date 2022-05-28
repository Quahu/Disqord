using System;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot;

/// <summary>
///     Represents a <see cref="string"/> prefix.
/// </summary>
public sealed class StringPrefix : IPrefix
{
    /// <summary>
    ///     Gets the <see cref="string"/> value to check for in the message content.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Gets the <see cref="StringComparison"/> to used when checking the message content.
    /// </summary>
    public StringComparison Comparison { get; }

    /// <summary>
    ///     Instantiates a new <see cref="StringPrefix"/> with the specified <see cref="string"/> value and <see cref="StringComparison"/>.
    /// </summary>
    /// <param name="value"> The <see cref="string"/> value. </param>
    /// <param name="comparison"> The <see cref="StringComparison"/>. </param>
    public StringPrefix(string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));

        Value = value;
        Comparison = comparison;
    }

    /// <inheritdoc/>
    public bool TryFind(IGatewayUserMessage message, out ReadOnlyMemory<char> output)
        => CommandUtilities.HasPrefix(message.Content.AsMemory(), Value, Comparison, out output);

    /// <inheritdoc/>
    public override int GetHashCode()
        => Value.GetHashCode(Comparison);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is StringPrefix prefix)
            return Value.Equals(prefix.Value, Comparison);

        if (obj is string value)
            return Value.Equals(value, Comparison);

        return false;
    }

    /// <inheritdoc/>
    public override string ToString()
        => Value;
}

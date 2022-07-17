using Qommon;

namespace Disqord;

/// <summary>
///     Represents a Discord authorization token.
/// </summary>
public abstract partial class Token
{
    /// <summary>
    ///     Gets the raw token string.
    /// </summary>
    public string RawValue { get; }

    private protected Token(string token)
    {
        Guard.IsNotNullOrWhiteSpace(token);

        RawValue = token;
    }

    // No authorization ctor.
    private Token()
    {
        RawValue = null!;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Token token)
            return RawValue == token.RawValue;

        if (obj is string rawValue)
            return RawValue == rawValue;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return RawValue?.GetHashCode() ?? 0;
    }

    /// <summary>
    ///     Gets the appropriately prefixed format of the token used for authorization headers.
    /// </summary>
    public abstract string? GetAuthorization();

    public static bool operator ==(Token? left, Token? right)
    {
        return left?.RawValue == right?.RawValue;
    }

    public static bool operator !=(Token? left, Token? right)
    {
        return left?.RawValue != right?.RawValue;
    }
}
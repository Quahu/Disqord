namespace Disqord;

/// <summary>
///     Represents a bearer token used with OAuth2.
/// </summary>
public sealed class BearerToken : Token
{
    internal BearerToken(string token)
        : base(token)
    { }

    /// <inheritdoc/>
    public override string GetAuthorization()
        => $"Bearer {RawValue}";
}
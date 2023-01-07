namespace Disqord.Voice.Api;

/// <summary>
///     Represents known Discord encryption modes.
/// </summary>
public static class KnownEncryptionModes
{
    /// <summary>
    ///     The <c>xsalsa20_poly1305</c> encryption mode.
    /// </summary>
    public const string XSalsa20Poly1305 = "xsalsa20_poly1305";

    /// <summary>
    ///     The <c>xsalsa20_poly1305_suffix</c> encryption mode.
    /// </summary>
    public const string XSalsa20Poly1305Suffix = "xsalsa20_poly1305_suffix";

    /// <summary>
    ///     The <c>xsalsa20_poly1305_lite</c> encryption mode.
    /// </summary>
    public const string XSalsa20Poly1305Lite = "xsalsa20_poly1305_lite";
}

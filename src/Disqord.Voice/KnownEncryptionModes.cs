namespace Disqord.Voice;

/// <summary>
///     Represents known Discord encryption modes.
/// </summary>
public static class KnownEncryptionModes
{
    /// <summary>
    ///     The <c>aead_aes256_gcm_rtpsize</c> encryption mode.
    /// </summary>
    public const string AEADAes256GcmRtpSize = "aead_aes256_gcm_rtpsize";

    /// <summary>
    ///     The <c>aead_xchacha20_poly1305_rtpsize</c> encryption mode.
    /// </summary>
    public const string AEADXChaCha20Poly1305RtpSize = "aead_xchacha20_poly1305_rtpsize";

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

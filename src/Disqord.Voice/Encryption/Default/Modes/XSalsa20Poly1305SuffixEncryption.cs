using System;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305Suffix"/> encryption.
/// </summary>
[Obsolete("This encryption mode has been deprecated by Discord.")]
public sealed class XSalsa20Poly1305SuffixEncryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305Suffix;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.XSalsa20Poly1305MacLength + Sodium.XSalsa20Poly1305NonceLength;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.XSalsa20Poly1305NonceLength]);
        Sodium.GetRandomBytes(nonce);

        Sodium.Encrypt(encryptedAudio[..^Sodium.XSalsa20Poly1305NonceLength], audio, nonce, key);
        nonce.CopyTo(encryptedAudio[^Sodium.XSalsa20Poly1305NonceLength..]);
    }
}

using System;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305"/> encryption.
/// </summary>
[Obsolete("This encryption mode has been deprecated by Discord.")]
public sealed class XSalsa20Poly1305Encryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.XSalsa20Poly1305MacLength;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.XSalsa20Poly1305NonceLength]);
        rtpHeader.CopyTo(nonce);

        Sodium.Encrypt(encryptedAudio, audio, nonce, key);
    }
}

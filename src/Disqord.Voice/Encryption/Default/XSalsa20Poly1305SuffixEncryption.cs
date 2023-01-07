using System;
using Disqord.Voice.Api;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305Suffix"/> encryption.
/// </summary>
public sealed class XSalsa20Poly1305SuffixEncryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305Suffix;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.MacLength + Sodium.NonceLength;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.NonceLength]);
        Sodium.GetRandomBytes(nonce);

        Sodium.Encrypt(encryptedAudio[..^Sodium.NonceLength], audio, nonce, key);
        nonce.CopyTo(encryptedAudio[^Sodium.NonceLength..]);
    }
}

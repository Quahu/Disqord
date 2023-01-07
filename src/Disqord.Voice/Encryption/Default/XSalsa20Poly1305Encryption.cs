using System;
using Disqord.Voice.Api;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305"/> encryption.
/// </summary>
public sealed class XSalsa20Poly1305Encryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.MacLength;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.NonceLength]);
        rtpHeader.CopyTo(nonce);

        Sodium.Encrypt(encryptedAudio, audio, nonce, key);
    }
}

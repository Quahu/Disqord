using System;
using System.Buffers.Binary;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305Lite"/> encryption.
/// </summary>
[Obsolete("This encryption mode has been deprecated by Discord.")]
public sealed class XSalsa20Poly1305LiteEncryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305Lite;

    private uint _nonceValue;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.XSalsa20Poly1305MacLength + 4;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.XSalsa20Poly1305NonceLength]);
        BinaryPrimitives.WriteUInt32BigEndian(nonce, _nonceValue);

        Sodium.Encrypt(encryptedAudio[..^4], audio, nonce, key);
        nonce[..4].CopyTo(encryptedAudio[^4..]);

        _nonceValue++;
    }
}

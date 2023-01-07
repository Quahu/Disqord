using System;
using System.Buffers.Binary;
using Disqord.Voice.Api;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.XSalsa20Poly1305Lite"/> encryption.
/// </summary>
public sealed class XSalsa20Poly1305LiteEncryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.XSalsa20Poly1305Lite;

    private uint _value = uint.MaxValue - 1;

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + Sodium.MacLength + 4;
    }

    /// <inheritdoc/>
    public void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[Sodium.NonceLength]);
        BinaryPrimitives.WriteUInt32BigEndian(nonce, _value);

        Sodium.Encrypt(encryptedAudio[..^4], audio, nonce, key);
        nonce[..4].CopyTo(encryptedAudio[^4..]);

        _value++;
    }
}

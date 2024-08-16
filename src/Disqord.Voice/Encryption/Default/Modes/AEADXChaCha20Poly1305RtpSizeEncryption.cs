using System;
using System.Buffers.Binary;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.AEADXChaCha20Poly1305RtpSize"/> encryption.
/// </summary>
public sealed class AEADXChaCha20Poly1305RtpSizeEncryption : IVoiceEncryption
{
    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.AEADXChaCha20Poly1305RtpSize;

    private uint _nonceValue;

    private readonly int _npubbytes;
    private readonly int _abytes;

    public AEADXChaCha20Poly1305RtpSizeEncryption()
    {
        _npubbytes = Sodium.crypto_aead_xchacha20poly1305_ietf_npubbytes();
        _abytes = Sodium.crypto_aead_xchacha20poly1305_ietf_abytes();
    }

    /// <inheritdoc/>
    public int GetEncryptedLength(int length)
    {
        return length + _abytes + 4;
    }

    /// <inheritdoc/>
    public unsafe void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key)
    {
        var nonce = (stackalloc byte[_npubbytes]);
        BinaryPrimitives.WriteUInt32BigEndian(nonce, _nonceValue);
        nonce[..4].CopyTo(encryptedAudio[^4..]);

        fixed (byte* encryptedAudioPtr = encryptedAudio)
        fixed (byte* rtpHeaderPtr = rtpHeader)
        fixed (byte* audioPtr = audio)
        fixed (byte* noncePtr = nonce)
        fixed (byte* keyPtr = key)
        {
            var result = Sodium.crypto_aead_xchacha20poly1305_ietf_encrypt(c: encryptedAudioPtr, clen_p: null,
                m: audioPtr, mlen: (ulong) audio.Length,
                ad: rtpHeaderPtr, adlen: (ulong) rtpHeader.Length,
                nsec: null, npub: noncePtr, k: keyPtr);

            if (result != 0)
            {
                Sodium.CheckEncryptionResult(result);
            }
        }

        _nonceValue++;
    }
}

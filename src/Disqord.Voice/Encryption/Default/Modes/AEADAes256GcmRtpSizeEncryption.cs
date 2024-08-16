using System;
using System.Buffers.Binary;

namespace Disqord.Voice.Default;

/// <summary>
///     Represents the <see cref="KnownEncryptionModes.AEADAes256GcmRtpSize"/> encryption.
/// </summary>
public sealed class AEADAes256GcmRtpSizeEncryption : IVoiceEncryption
{
    /// <summary>
    ///     Checks whether this mode is supported by the host machine's hardware.
    /// </summary>
    public static bool IsSupported => Sodium.crypto_aead_aes256gcm_is_available();

    /// <inheritdoc/>
    public string ModeName => KnownEncryptionModes.AEADAes256GcmRtpSize;

    private uint _nonceValue;

    private readonly int _abytes;
    private readonly int _npubbytes;

    public AEADAes256GcmRtpSizeEncryption()
    {
        _abytes = Sodium.crypto_aead_aes256gcm_abytes();
        _npubbytes = Sodium.crypto_aead_aes256gcm_npubbytes();
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
            var result = Sodium.crypto_aead_aes256gcm_encrypt(c: encryptedAudioPtr, clen_p: null,
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

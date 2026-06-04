using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a type supporting a specific Discord voice encryption mode.
/// </summary>
public interface IVoiceEncryption
{
    /// <summary>
    ///     Gets the Discord mode name identifying this encryption.
    /// </summary>
    string ModeName { get; }

    /// <summary>
    ///     Gets the data length after encryption for the specified data length.
    /// </summary>
    /// <param name="length"> The data length. </param>
    /// <returns>
    ///     The data length after encryption.
    /// </returns>
    int GetEncryptedLength(int length);

    /// <summary>
    ///     Gets the data length after decryption for the specified encrypted data length.
    /// </summary>
    /// <param name="length"> The encrypted data length. </param>
    /// <returns>
    ///     The data length after decryption.
    /// </returns>
    int GetDecryptedLength(int length);

    /// <summary>
    ///     Encrypts <paramref name="audio"/> into <paramref name="encryptedAudio"/> using <paramref name="key"/>.
    /// </summary>
    /// <param name="rtpHeader"> The RTP header. </param>
    /// <param name="encryptedAudio"> The destination span. </param>
    /// <param name="audio"> The source span. </param>
    /// <param name="key"> The encryption key span. </param>
    void Encrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> encryptedAudio, ReadOnlySpan<byte> audio, ReadOnlySpan<byte> key);

    /// <summary>
    ///     Decrypts <paramref name="encryptedAudio"/> into <paramref name="audio"/> using <paramref name="key"/>.
    /// </summary>
    /// <param name="rtpHeader"> The RTP header. </param>
    /// <param name="audio"> The destination span. </param>
    /// <param name="encryptedAudio"> The source span. </param>
    /// <param name="key"> The encryption key span. </param>
    void Decrypt(ReadOnlySpan<byte> rtpHeader, Span<byte> audio, ReadOnlySpan<byte> encryptedAudio, ReadOnlySpan<byte> key);
}

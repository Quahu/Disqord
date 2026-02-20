using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE encryptor handle.
/// </summary>
public sealed unsafe class DaveEncryptor : IDisposable
{
    /// <summary>
    ///     Gets the native handle of this encryptor.
    /// </summary>
    public nint Handle
    {
        get
        {
            ThrowIfDisposed();
            return _handle;
        }
    }

    private nint _handle;
    private bool _isDisposed;

    /// <summary>
    ///     Creates a new DAVE encryptor.
    /// </summary>
    public DaveEncryptor()
        : this(Dave.EncryptorCreate())
    { }

    private DaveEncryptor(nint handle)
    {
        _handle = handle;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    /// <summary>
    ///     Gets whether the encryptor has a key ratchet.
    /// </summary>
    public bool HasKeyRatchet
    {
        get
        {
            ThrowIfDisposed();
            return Dave.EncryptorHasKeyRatchet(_handle);
        }
    }

    /// <summary>
    ///     Gets whether the encryptor is in passthrough mode.
    /// </summary>
    public bool IsPassthroughMode
    {
        get
        {
            ThrowIfDisposed();
            return Dave.EncryptorIsPassthroughMode(_handle);
        }
    }

    /// <summary>
    ///     Sets the key ratchet for encryption.
    /// </summary>
    public void SetKeyRatchet(DaveKeyRatchet keyRatchet)
    {
        ThrowIfDisposed();
        Dave.EncryptorSetKeyRatchet(_handle, keyRatchet.Handle);
    }

    /// <summary>
    ///     Enables or disables passthrough mode.
    /// </summary>
    public void SetPassthroughMode(bool passthroughMode)
    {
        ThrowIfDisposed();
        Dave.EncryptorSetPassthroughMode(_handle, passthroughMode);
    }

    /// <summary>
    ///     Associates an SSRC with a specific codec.
    /// </summary>
    public void AssignSsrcToCodec(uint ssrc, Dave.Codec codecType)
    {
        ThrowIfDisposed();
        Dave.EncryptorAssignSsrcToCodec(_handle, ssrc, codecType);
    }

    /// <summary>
    ///     Gets the current protocol version used by the encryptor.
    /// </summary>
    public ushort GetProtocolVersion()
    {
        ThrowIfDisposed();
        return Dave.EncryptorGetProtocolVersion(_handle);
    }

    /// <summary>
    ///     Calculates the maximum ciphertext size for a given plaintext frame size.
    /// </summary>
    public nuint GetMaxCiphertextByteSize(Dave.MediaType mediaType, nuint frameSize)
    {
        ThrowIfDisposed();
        return Dave.EncryptorGetMaxCiphertextByteSize(_handle, mediaType, frameSize);
    }

    /// <summary>
    ///     Encrypts a media frame.
    /// </summary>
    /// <param name="mediaType"> The media type (audio or video). </param>
    /// <param name="ssrc"> The SSRC of the stream. </param>
    /// <param name="frame"> The plaintext frame data. </param>
    /// <param name="encryptedFrame"> The output buffer for the encrypted frame. </param>
    /// <param name="bytesWritten"> The number of bytes written to the output buffer. </param>
    /// <returns> The result code indicating success or failure. </returns>
    public Dave.EncryptorResultCode Encrypt(Dave.MediaType mediaType, uint ssrc,
        ReadOnlySpan<byte> frame, Span<byte> encryptedFrame, out nuint bytesWritten)
    {
        ThrowIfDisposed();

        fixed (byte* framePtr = frame)
        fixed (byte* encryptedFramePtr = encryptedFrame)
        {
            var result = Dave.EncryptorEncrypt(_handle, mediaType, ssrc,
                framePtr, (nuint) frame.Length,
                encryptedFramePtr, (nuint) encryptedFrame.Length, out bytesWritten);

            return result;
        }
    }

    /// <summary>
    ///     Gets encryption statistics.
    /// </summary>
    public Dave.EncryptorStats GetStats(Dave.MediaType mediaType)
    {
        ThrowIfDisposed();

        Dave.EncryptorGetStats(_handle, mediaType, out var stats);
        return stats;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Dave.EncryptorDestroy(_handle);
        _handle = 0;
    }
}

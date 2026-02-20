using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE decryptor handle.
/// </summary>
public sealed unsafe class DaveDecryptor : IDisposable
{
    /// <summary>
    ///     Gets the native handle of this decryptor.
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
    ///     Creates a new DAVE decryptor.
    /// </summary>
    public DaveDecryptor()
        : this(Dave.DecryptorCreate())
    { }

    private DaveDecryptor(nint handle)
    {
        _handle = handle;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    /// <summary>
    ///     Transitions the decryptor to use a new key ratchet.
    /// </summary>
    public void TransitionToKeyRatchet(DaveKeyRatchet keyRatchet)
    {
        ThrowIfDisposed();
        Dave.DecryptorTransitionToKeyRatchet(_handle, keyRatchet.Handle);
    }

    /// <summary>
    ///     Transitions to or from passthrough mode.
    /// </summary>
    public void TransitionToPassthroughMode(bool passthroughMode)
    {
        ThrowIfDisposed();
        Dave.DecryptorTransitionToPassthroughMode(_handle, passthroughMode);
    }

    /// <summary>
    ///     Decrypts an encrypted media frame.
    /// </summary>
    /// <param name="mediaType"> The media type (audio or video). </param>
    /// <param name="encryptedFrame"> The encrypted frame data. </param>
    /// <param name="frame"> The output buffer for the decrypted frame. </param>
    /// <param name="bytesWritten"> The number of bytes written to the output buffer. </param>
    /// <returns> The result code indicating success or failure. </returns>
    public Dave.DecryptorResultCode Decrypt(Dave.MediaType mediaType,
        ReadOnlySpan<byte> encryptedFrame, Span<byte> frame, out nuint bytesWritten)
    {
        ThrowIfDisposed();

        fixed (byte* encryptedFramePtr = encryptedFrame)
        fixed (byte* framePtr = frame)
        {
            var result = Dave.DecryptorDecrypt(_handle, mediaType,
                encryptedFramePtr, (nuint) encryptedFrame.Length,
                framePtr, (nuint) frame.Length, out bytesWritten);

            return result;
        }
    }

    /// <summary>
    ///     Calculates the maximum plaintext size for a given ciphertext frame size.
    /// </summary>
    public nuint GetMaxPlaintextByteSize(Dave.MediaType mediaType, nuint encryptedFrameSize)
    {
        ThrowIfDisposed();
        return Dave.DecryptorGetMaxPlaintextByteSize(_handle, mediaType, encryptedFrameSize);
    }

    /// <summary>
    ///     Gets decryption statistics.
    /// </summary>
    public Dave.DecryptorStats GetStats(Dave.MediaType mediaType)
    {
        ThrowIfDisposed();

        Dave.DecryptorGetStats(_handle, mediaType, out var stats);
        return stats;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Dave.DecryptorDestroy(_handle);
        _handle = 0;
    }
}

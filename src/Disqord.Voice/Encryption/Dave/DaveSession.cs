using System;
using System.Buffers.Text;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE session handle.
/// </summary>
public sealed unsafe class DaveSession : IDisposable
{
    // Max 20 decimal digits + null terminator for a ulong/Snowflake.
    private const int MaxSnowflakeUtf8Length = 21;

    /// <summary>
    ///     Gets the native handle of this session.
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
    private Dave.MlsFailureCallback? _failureCallback;
    private bool _isDisposed;

    private DaveSession(nint handle, Dave.MlsFailureCallback? failureCallback)
    {
        _handle = handle;
        _failureCallback = failureCallback;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    /// <summary>
    ///     Creates a new DAVE session.
    /// </summary>
    /// <param name="failureCallback"> The callback invoked on MLS failures. </param>
    /// <returns> A new <see cref="DaveSession"/>. </returns>
    public static DaveSession Create(Dave.MlsFailureCallback? failureCallback = null)
    {
        var handle = Dave.SessionCreate(0, null, failureCallback!, 0);
        return new DaveSession(handle, failureCallback);
    }

    /// <summary>
    ///     Initializes the session with protocol version and group information.
    /// </summary>
    /// <param name="version"> The protocol version to use. </param>
    /// <param name="groupId"> The group identifier (guild ID). </param>
    /// <param name="selfUserId"> The user ID of the local user. </param>
    public void Init(ushort version, Snowflake groupId, Snowflake selfUserId)
    {
        ThrowIfDisposed();

        var userIdBuffer = stackalloc byte[MaxSnowflakeUtf8Length];
        FormatSnowflake(selfUserId, userIdBuffer);
        Dave.SessionInit(_handle, version, groupId, userIdBuffer);
    }

    /// <summary>
    ///     Resets the session state.
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();
        Dave.SessionReset(_handle);
    }

    /// <summary>
    ///     Sets the protocol version for the session.
    /// </summary>
    public void SetProtocolVersion(ushort version)
    {
        ThrowIfDisposed();
        Dave.SessionSetProtocolVersion(_handle, version);
    }

    /// <summary>
    ///     Gets the current protocol version of the session.
    /// </summary>
    public ushort GetProtocolVersion()
    {
        ThrowIfDisposed();
        return Dave.SessionGetProtocolVersion(_handle);
    }

    /// <summary>
    ///     Retrieves the authenticator from the last MLS epoch.
    /// </summary>
    /// <returns> A <see cref="DaveNativeBuffer"/> that must be disposed. </returns>
    public DaveNativeBuffer GetLastEpochAuthenticator()
    {
        ThrowIfDisposed();

        Dave.SessionGetLastEpochAuthenticator(_handle, out var authenticator, out var length);
        return new DaveNativeBuffer(authenticator, (int) length);
    }

    /// <summary>
    ///     Sets the external sender credentials for the session.
    /// </summary>
    public void SetExternalSender(ReadOnlySpan<byte> externalSender)
    {
        ThrowIfDisposed();

        fixed (byte* ptr = externalSender)
        {
            Dave.SessionSetExternalSender(_handle, ptr, (nuint) externalSender.Length);
        }
    }

    /// <summary>
    ///     Processes MLS proposals and generates commit/welcome messages.
    /// </summary>
    /// <param name="proposals"> The serialized proposal bytes. </param>
    /// <param name="recognizedUserIds"> The recognized user IDs. </param>
    /// <returns> A <see cref="DaveNativeBuffer"/> that must be disposed. </returns>
    public DaveNativeBuffer ProcessProposals(ReadOnlySpan<byte> proposals, ReadOnlySpan<Snowflake> recognizedUserIds)
    {
        ThrowIfDisposed();

        var stringBuffer = stackalloc byte[recognizedUserIds.Length * MaxSnowflakeUtf8Length];
        var pointerBuffer = stackalloc byte*[recognizedUserIds.Length];
        FormatSnowflakes(recognizedUserIds, stringBuffer, pointerBuffer);

        fixed (byte* proposalsPtr = proposals)
        {
            Dave.SessionProcessProposals(_handle, proposalsPtr, (nuint) proposals.Length,
                pointerBuffer, (nuint) recognizedUserIds.Length,
                out var commitWelcomeBytes, out var commitWelcomeBytesLength);

            return new DaveNativeBuffer(commitWelcomeBytes, (int) commitWelcomeBytesLength);
        }
    }

    /// <summary>
    ///     Processes an incoming MLS commit message.
    /// </summary>
    /// <returns> A <see cref="DaveCommitResult"/> that must be disposed. </returns>
    public DaveCommitResult ProcessCommit(ReadOnlySpan<byte> commit)
    {
        ThrowIfDisposed();

        fixed (byte* commitPtr = commit)
        {
            var handle = Dave.SessionProcessCommit(_handle, commitPtr, (nuint) commit.Length);
            return new DaveCommitResult(handle);
        }
    }

    /// <summary>
    ///     Processes an incoming MLS welcome message to join a group.
    /// </summary>
    /// <returns> A <see cref="DaveWelcomeResult"/> that must be disposed. Check <see cref="DaveWelcomeResult.IsValid"/>. </returns>
    public DaveWelcomeResult ProcessWelcome(ReadOnlySpan<byte> welcome, ReadOnlySpan<Snowflake> recognizedUserIds)
    {
        ThrowIfDisposed();

        var stringBuffer = stackalloc byte[recognizedUserIds.Length * MaxSnowflakeUtf8Length];
        var pointerBuffer = stackalloc byte*[recognizedUserIds.Length];
        FormatSnowflakes(recognizedUserIds, stringBuffer, pointerBuffer);

        fixed (byte* welcomePtr = welcome)
        {
            var handle = Dave.SessionProcessWelcome(_handle, welcomePtr, (nuint) welcome.Length,
                pointerBuffer, (nuint) recognizedUserIds.Length);

            return new DaveWelcomeResult(handle);
        }
    }

    /// <summary>
    ///     Gets the marshalled MLS key package for this session.
    /// </summary>
    /// <returns> A <see cref="DaveNativeBuffer"/> that must be disposed. </returns>
    public DaveNativeBuffer GetMarshalledKeyPackage()
    {
        ThrowIfDisposed();

        Dave.SessionGetMarshalledKeyPackage(_handle, out var keyPackage, out var length);
        return new DaveNativeBuffer(keyPackage, (int) length);
    }

    /// <summary>
    ///     Gets a key ratchet for a specific user in the session.
    /// </summary>
    /// <returns> A <see cref="DaveKeyRatchet"/> that must be disposed. </returns>
    public DaveKeyRatchet GetKeyRatchet(Snowflake userId)
    {
        ThrowIfDisposed();

        var userIdBuffer = stackalloc byte[MaxSnowflakeUtf8Length];
        FormatSnowflake(userId, userIdBuffer);
        var handle = Dave.SessionGetKeyRatchet(_handle, userIdBuffer);
        return new DaveKeyRatchet(handle);
    }

    /// <summary>
    ///     Computes a pairwise fingerprint for identity verification with another user.
    /// </summary>
    public void GetPairwiseFingerprint(ushort version, Snowflake userId,
        Dave.PairwiseFingerprintCallback callback)
    {
        ThrowIfDisposed();

        var userIdBuffer = stackalloc byte[MaxSnowflakeUtf8Length];
        FormatSnowflake(userId, userIdBuffer);
        Dave.SessionGetPairwiseFingerprint(_handle, version, userIdBuffer, callback, 0);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Dave.SessionDestroy(_handle);
        _handle = 0;
        _failureCallback = null;
    }

    private static void FormatSnowflake(Snowflake snowflake, byte* destination)
    {
        Utf8Formatter.TryFormat(snowflake, new Span<byte>(destination, MaxSnowflakeUtf8Length - 1), out var bytesWritten);
        destination[bytesWritten] = 0;
    }

    private static void FormatSnowflakes(ReadOnlySpan<Snowflake> snowflakes, byte* stringBuffer, byte** pointerBuffer)
    {
        for (var i = 0; i < snowflakes.Length; i++)
        {
            var strStart = stringBuffer + i * MaxSnowflakeUtf8Length;
            pointerBuffer[i] = strStart;
            FormatSnowflake(snowflakes[i], strStart);
        }
    }
}

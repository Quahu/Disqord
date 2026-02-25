using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE commit result handle.
/// </summary>
public unsafe struct DaveCommitResult : IDisposable
{
    /// <summary>
    ///     Gets the native handle of this commit result.
    /// </summary>
    public readonly nint Handle => _handle;

    private nint _handle;

    internal DaveCommitResult(nint handle)
    {
        _handle = handle;
    }

    /// <summary>
    ///     Gets whether processing the commit failed.
    /// </summary>
    public readonly bool IsFailed => Dave.CommitResultIsFailed(_handle);

    /// <summary>
    ///     Gets whether the commit should be ignored.
    /// </summary>
    public readonly bool IsIgnored => Dave.CommitResultIsIgnored(_handle);

    /// <summary>
    ///     Gets the list of member IDs in the roster after the commit.
    /// </summary>
    public readonly Snowflake[] GetRosterMemberIds()
    {
        Dave.CommitResultGetRosterMemberIds(_handle, out var rosterIds, out var length);
        try
        {
            // Snowflake has the same layout as ulong.
            return new ReadOnlySpan<Snowflake>(rosterIds, (int) length).ToArray();
        }
        finally
        {
            Dave.Free(rosterIds);
        }
    }

    /// <summary>
    ///     Gets the signature for a specific roster member.
    /// </summary>
    public readonly DaveNativeBuffer GetRosterMemberSignature(ulong rosterId)
    {
        Dave.CommitResultGetRosterMemberSignature(_handle, rosterId, out var signature, out var signatureLength);
        return new DaveNativeBuffer(signature, (int) signatureLength);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        var handle = _handle;
        if (handle == 0)
            return;

        _handle = 0;
        Dave.CommitResultDestroy(handle);
    }
}

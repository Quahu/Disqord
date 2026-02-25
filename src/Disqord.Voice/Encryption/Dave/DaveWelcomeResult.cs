using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE welcome result handle.
/// </summary>
public unsafe struct DaveWelcomeResult : IDisposable
{
    /// <summary>
    ///     Gets the native handle of this welcome result.
    /// </summary>
    public readonly nint Handle => _handle;

    /// <summary>
    ///     Gets whether the welcome result is valid (i.e., the client successfully joined the group).
    /// </summary>
    public readonly bool IsValid => _handle != 0;

    private nint _handle;

    internal DaveWelcomeResult(nint handle)
    {
        _handle = handle;
    }

    /// <summary>
    ///     Gets the list of member IDs in the roster from the welcome message.
    /// </summary>
    public readonly Snowflake[] GetRosterMemberIds()
    {
        Dave.WelcomeResultGetRosterMemberIds(_handle, out var rosterIds, out var length);
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
        Dave.WelcomeResultGetRosterMemberSignature(_handle, rosterId, out var signature, out var signatureLength);
        return new DaveNativeBuffer(signature, (int) signatureLength);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        var handle = _handle;
        if (handle == 0)
            return;

        _handle = 0;
        Dave.WelcomeResultDestroy(handle);
    }
}

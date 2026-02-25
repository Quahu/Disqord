using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a managed wrapper over a native DAVE key ratchet handle.
/// </summary>
public struct DaveKeyRatchet : IDisposable
{
    /// <summary>
    ///     Gets the native handle of this key ratchet.
    /// </summary>
    public readonly nint Handle => _handle;

    private nint _handle;

    internal DaveKeyRatchet(nint handle)
    {
        _handle = handle;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        var handle = _handle;
        if (handle == 0)
            return;

        _handle = 0;
        Dave.KeyRatchetDestroy(handle);
    }
}

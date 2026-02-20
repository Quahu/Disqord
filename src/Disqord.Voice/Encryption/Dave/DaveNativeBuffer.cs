using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a disposable view over native DAVE-allocated memory.
///     The underlying memory is freed when this value is disposed.
/// </summary>
public unsafe ref struct DaveNativeBuffer
{
    /// <summary>
    ///     Gets a read-only span over the native memory.
    /// </summary>
    public readonly ReadOnlySpan<byte> Span => new(_ptr, Length);

    /// <summary>
    ///     Gets the length of the buffer in bytes.
    /// </summary>
    public int Length { get; }

    private byte* _ptr;

    internal DaveNativeBuffer(byte* ptr, int length)
    {
        _ptr = ptr;
        Length = length;
    }

    /// <summary>
    ///     Frees the underlying native memory.
    /// </summary>
    public void Dispose()
    {
        if (_ptr != null)
        {
            Dave.Free(_ptr);
            _ptr = null;
        }
    }
}

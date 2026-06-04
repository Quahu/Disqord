using System;
using System.Collections.Generic;
using System.Threading;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents a source of Opus-encoded audio packets.
///     Each element is a single 20ms Opus frame as a <see cref="ReadOnlyMemory{T}"/> of bytes.
/// </summary>
public interface IAudioSource : IAsyncEnumerable<ReadOnlyMemory<byte>>
{
    /// <summary>
    ///     Wraps an existing <see cref="IAsyncEnumerable{T}"/> of Opus packets
    ///     as an <see cref="IAudioSource"/>.
    /// </summary>
    /// <param name="source"> The async enumerable yielding 20ms Opus-encoded audio packets. </param>
    /// <returns> An <see cref="IAudioSource"/> wrapping the provided enumerable. </returns>
    static IAudioSource Create(IAsyncEnumerable<ReadOnlyMemory<byte>> source)
    {
        if (source is IAudioSource audioSource)
            return audioSource;

        return new WrappedAudioSource(source);
    }

    private sealed class WrappedAudioSource(IAsyncEnumerable<ReadOnlyMemory<byte>> source) : IAudioSource
    {
        public IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return source.GetAsyncEnumerator(cancellationToken);
        }
    }
}

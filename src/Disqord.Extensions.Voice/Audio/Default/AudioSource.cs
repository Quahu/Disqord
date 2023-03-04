using System;
using System.Collections.Generic;
using System.Threading;
using Qommon.Metadata;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents a type responsible for yielding Opus audio.
/// </summary>
public abstract class AudioSource : IAsyncEnumerable<Memory<byte>>, IThreadSafeMetadata
{
    /// <summary>
    ///     Gets an enumerator which should yield 20 milliseconds worth of Opus audio packets.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     The enumerator.
    /// </returns>
    public abstract IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken);
}

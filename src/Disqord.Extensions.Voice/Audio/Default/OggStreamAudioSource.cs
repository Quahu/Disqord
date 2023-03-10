using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents an audio source that can read the Ogg format.
/// </summary>
public class OggStreamAudioSource : AudioSource
{
    private readonly Stream _stream;

    /// <summary>
    ///     Instantiates a new <see cref="OggStreamAudioSource"/>.
    /// </summary>
    /// <param name="stream"> The Ogg stream. </param>
    public OggStreamAudioSource(Stream stream)
    {
        _stream = stream;
    }

    /// <inheritdoc/>
    public override IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        return new OggReader(_stream).GetAsyncEnumerator(cancellationToken);
    }
}

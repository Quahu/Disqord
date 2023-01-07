using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Pooling;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents an audio source that can read the Ogg format.
/// </summary>
public class OggStreamAudioSource : AudioSource
{
    private readonly OggReader _reader;

    /// <summary>
    ///     Instantiates a new <see cref="OggStreamAudioSource"/>.
    /// </summary>
    /// <param name="stream"> The Ogg stream. </param>
    public OggStreamAudioSource(Stream stream)
    {
        _reader = new(stream);
    }

    private class Enumerator : IAsyncEnumerator<Memory<byte>>
    {
        public Memory<byte> Current => _enumerator.Current;

        private readonly IAsyncEnumerator<RentedArray<byte>> _enumerator;
        private RentedArray<byte>? _lastPacket;

        public Enumerator(OggReader reader, CancellationToken cancellationToken)
        {
            _enumerator = reader.GetAsyncEnumerator(cancellationToken);
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (!await _enumerator.MoveNextAsync().ConfigureAwait(false))
                return false;

            if (_lastPacket != null)
            {
                _lastPacket.Value.Dispose();
            }

            _lastPacket = _enumerator.Current;
            return true;
        }

        public ValueTask DisposeAsync()
        {
            if (_lastPacket != null)
            {
                _lastPacket.Value.Dispose();
            }

            return _enumerator.DisposeAsync();
        }
    }

    /// <inheritdoc/>
    public override IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        return new Enumerator(_reader, cancellationToken);
    }
}

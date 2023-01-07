using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Voice;

namespace BasicVoice;

/// <summary>
///     Represents an audio source that uses FFmpeg to produce
///     the Opus audio packets. The input stream can be any format
///     that FFmpeg is able to convert (virtually anything).
/// </summary>
/// <remarks>
///     Note that this is the simplest possible implementation,
///     it does not account for errors and does not
///     propagate them in any way.
/// </remarks>
public class FfmpegAudioSource : AudioSource
{
    private readonly Process _process;
    private readonly AudioSource _ogg;
    private readonly CancellationTokenSource _cts;

    /// <summary>
    ///     Instantiates a new <see cref="FfmpegAudioSource"/> with the specified audio stream.
    /// </summary>
    /// <param name="stream"> The audio stream </param>
    public FfmpegAudioSource(Stream stream)
    {
        _process = Process.Start(new ProcessStartInfo
        {
            // Assumes ffmpeg is in PATH.
            FileName = "ffmpeg",

            // Could be customized to, for example, support changing the volume.
            Arguments = "-hide_banner -loglevel error -i pipe:0 -c:a libopus -frame_duration 20 -application audio -f oga pipe:1",

            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false
        })!;

        _cts = new();
        _ = stream.CopyToAsync(_process.StandardInput.BaseStream, _cts.Token)
            .ContinueWith(_ => _process.StandardInput.BaseStream.Dispose(), TaskContinuationOptions.RunContinuationsAsynchronously);

        _ogg = new OggStreamAudioSource(_process.StandardOutput.BaseStream);
    }

    /// <inheritdoc/>
    public override async IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        await foreach (var packet in _ogg.WithCancellation(cancellationToken))
        {
            yield return packet;
        }

        _cts.Cancel();
        _cts.Dispose();
        var exitTask = _process.WaitForExitAsync(default);
        _process.Kill();
        await exitTask;
    }
}

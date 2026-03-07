using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Voice;

namespace BasicVoice;

public class FFmpegAudioSource(Stream stream) : IAudioSource
{
    private const string FFmpegPath = "ffmpeg";

    private static void PopulateFFmpegArguments(Collection<string> arguments)
    {
        arguments.Add("-loglevel");
        arguments.Add("error");

        // stdin as input
        arguments.Add("-i");
        arguments.Add("pipe:0");

        arguments.Add("-c:a");
        arguments.Add("libopus");

        arguments.Add("-application");
        arguments.Add("audio");

        arguments.Add("-frame_duration");
        arguments.Add("20");

        arguments.Add("-f");
        arguments.Add("oga");

        // stdout as output
        arguments.Add("pipe:1");
    }

    public async IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        await using (stream)
        {
            var ffmpegStartInfo = new ProcessStartInfo
            {
                FileName = FFmpegPath,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            PopulateFFmpegArguments(ffmpegStartInfo.ArgumentList);

            using (var ffmpeg = Process.Start(ffmpegStartInfo)!)
            {
                try
                {
                    var readErrorTask = ffmpeg.StandardError.ReadToEndAsync(cancellationToken);
                    var copyStreamTask = CopyStreamToFFmpegStdinAsync(stream, ffmpeg.StandardInput.BaseStream, cancellationToken);

                    var ogg = new OggStreamAudioSource(ffmpeg.StandardOutput.BaseStream);
                    await foreach (var packet in ogg.WithCancellation(cancellationToken))
                    {
                        yield return packet;
                    }

                    if (ffmpeg.ExitCode != 0)
                    {
                        var error = await readErrorTask;
                        error = !string.IsNullOrWhiteSpace(error)
                            ? error.ReplaceLineEndings(";")
                            : "unknown error";

                        throw new Exception($"FFmpeg exited with code {ffmpeg.ExitCode} ({error}).");
                    }

                    await copyStreamTask;
                }
                finally
                {
                    await CleanUpFFmpegAsync(ffmpeg);
                }
            }
        }
    }

    private static async Task CleanUpFFmpegAsync(Process ffmpeg)
    {
        var exitTask = ffmpeg.WaitForExitAsync(default);

        // Close the data streams; order matters here.
        await ffmpeg.StandardOutput.BaseStream.DisposeAsync();
        await ffmpeg.StandardError.BaseStream.DisposeAsync();
        await ffmpeg.StandardInput.BaseStream.DisposeAsync();

        try
        {
            // Give FFmpeg some time to exit gracefully.
            await exitTask.WaitAsync(TimeSpan.FromSeconds(1), CancellationToken.None);
        }
        catch (TimeoutException)
        {
            try
            {
                ffmpeg.Kill();
            }
            catch { }
        }

        await exitTask;
    }

    private static async Task CopyStreamToFFmpegStdinAsync(Stream stream, Stream stdin, CancellationToken cancellationToken)
    {
        await Task.Yield();
        await stream.CopyToAsync(stdin, cancellationToken);

        // Close stdin to indicate we've fed FFmpeg all the data
        await stdin.DisposeAsync();
    }
}

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Voice;

namespace BasicVoice;

// Represents an audio source implementation that uses FFmpeg to produce
// the Opus audio packets. The input stream can be any format
// that FFmpeg is able to convert (virtually anything).
public class FFmpegAudioSource : AudioSource
{
    private readonly Stream _stream;

    // The path to the FFmpeg executable.
    // For this default value to work
    // put FFmpeg in the bot's working directory or add it to PATH.
    private const string FFmpegPath = "ffmpeg";

    public FFmpegAudioSource(Stream stream)
    {
        _stream = stream;
    }

    private static void PopulateFFmpegArguments(Collection<string> arguments)
    {
        // You can add your own arguments here.
        // For example, you could add support for changing the volume using:
        // arguments.Add("-filter:a");
        // arguments.Add("\"volume=0.5\"");
        //
        // You could also set the volume float via the constructor.
        // You'd then make PopulateFFmpegArguments non-static and set it dynamically.
        // arguments.Add("-filter:a");
        // arguments.Add($"\"volume={Volume}\"");
        //
        // Keep in mind that the position of some FFmpeg arguments matters!
        //
        // Depending on their position in the argument string,
        // they can behave differently.
        // For example, `-ss` placed before `-i` is input seeking
        // and placed after `-i` is output seeking.

        // [Recommended]: Sets the log level to error.
        arguments.Add("-loglevel");
        arguments.Add("error");

        // [Required]: Sets the input to stdin.
        arguments.Add("-i");
        arguments.Add("pipe:0");

        // [Required]: Sets the audio codec to libopus.
        arguments.Add("-c:a");
        arguments.Add("libopus");

        // [Required]: Sets the Opus application type.
        arguments.Add("-application");
        arguments.Add("audio");

        // [Required]: Sets the Opus frame duration to 20ms.
        arguments.Add("-frame_duration");
        arguments.Add("20");

        // [Required]: Sets the format to Ogg audio.
        arguments.Add("-f");
        arguments.Add("oga");

        // [Required]: Sets the output to stdout.
        // Must be the last argument.
        arguments.Add("pipe:1");
    }

    public override async IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        // If the stream is seekable, rewind it to the beginning.
        if (_stream.CanSeek)
        {
            // This project doesn't reuse audio sources, but if it did,
            // this code would make this audio source reusable as long as
            // the stream passed in allows for rewinding.
            _stream.Seek(0, SeekOrigin.Begin);
        }

        var ffmpegStartInfo = new ProcessStartInfo
        {
            FileName = FFmpegPath,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        PopulateFFmpegArguments(ffmpegStartInfo.ArgumentList);

        using (var ffmpeg = Process.Start(ffmpegStartInfo)!)
        {
            try
            {
                // Start the error reading and stream copying tasks.
                var readErrorTask = ReadFFmpegStderrAsync(ffmpeg.StandardError.BaseStream, ffmpeg.StandardError.CurrentEncoding, cancellationToken);
                var copyStreamTask = CopyStreamToFFmpegStdinAsync(_stream, ffmpeg.StandardInput.BaseStream, cancellationToken);

                // Create an Ogg source for stdout.
                var ogg = new OggStreamAudioSource(ffmpeg.StandardOutput.BaseStream);

                // Enumerate the Ogg source to yield Ogg packets.
                await foreach (var packet in ogg.WithCancellation(cancellationToken))
                {
                    yield return packet;
                }

                // Check if FFmpeg exited with an error.
                if (ffmpeg.ExitCode != 0)
                {
                    var error = await readErrorTask;
                    error = !string.IsNullOrWhiteSpace(error)
                        ? error.ReplaceLineEndings(";")
                        : "unknown error";

                    throw new Exception($"FFmpeg exited with code {ffmpeg.ExitCode} ({error}).");
                }

                // Propagate the error from stream copying, if any.
                await copyStreamTask;
            }
            finally
            {
                await CleanUpFFmpegAsync(ffmpeg);
            }
        }
    }

    private static async Task CleanUpFFmpegAsync(Process ffmpeg)
    {
        // Start the exit waiting task.
        var exitTask = ffmpeg.WaitForExitAsync(default);

        // Close the data streams; order matters here.
        await ffmpeg.StandardOutput.BaseStream.DisposeAsync();
        await ffmpeg.StandardError.BaseStream.DisposeAsync();
        await ffmpeg.StandardInput.BaseStream.DisposeAsync();

        try
        {
            // Give FFmpeg some time to close gracefully.
            await exitTask.WaitAsync(TimeSpan.FromSeconds(1), CancellationToken.None);
        }
        catch (TimeoutException)
        {
            try
            {
                // Kill FFmpeg if it took too long to close.
                ffmpeg.Kill();
            }
            catch { }
        }

        await exitTask;
    }

    private static async Task CopyStreamToFFmpegStdinAsync(Stream stream, Stream stdin, CancellationToken cancellationToken)
    {
        // Yield, so the code below runs in background.
        await Task.Yield();

        // Copy the stream to stdin.
        await stream.CopyToAsync(stdin, cancellationToken);

        // Close stdin after copying is complete.
        await stdin.DisposeAsync();
    }

    // This method may seem complex, but it's actually
    // just a bunch of boilerplate code that's necessary
    // to efficiently, correctly, and asynchronously
    // capture stderr of any size and convert it into a string.
    private static async Task<string?> ReadFFmpegStderrAsync(Stream stderr, Encoding encoding, CancellationToken cancellationToken)
    {
        // Yield, so the code below runs in background.
        await Task.Yield();

        StringBuilder? sb = null;
        var buffer = new byte[256];
        try
        {
            int bytesRead;
            while ((bytesRead = await stderr.ReadAsync(buffer, cancellationToken)) != 0)
            {
                static void AppendChars(StringBuilder sb, Encoding errorEncoding, ReadOnlySpan<byte> bufferSpan)
                {
                    var charCount = errorEncoding.GetCharCount(bufferSpan);
                    var chars = ArrayPool<char>.Shared.Rent(charCount);
                    var charSpan = chars.AsSpan(0, charCount);
                    try
                    {
                        errorEncoding.GetChars(bufferSpan, charSpan);
                        sb.Append(charSpan);
                    }
                    finally
                    {
                        ArrayPool<char>.Shared.Return(chars);
                    }
                }

                AppendChars(sb ??= new(), encoding, buffer.AsSpan(0, bytesRead));
            }
        }
        catch
        {
            // Ignored, so that we can return any errors read thus far.
        }

        return sb?.ToString();
    }
}

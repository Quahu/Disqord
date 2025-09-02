using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class StreamConverter : JsonConverter<Stream>
{
    // This header works regardless of the actual type of the attachment.
    public const string Header = "data:image/jpeg;base64,";

    public override Stream? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, Stream stream, JsonSerializerOptions options)
    {
        Guard.CanRead(stream);

        StringBuilder base64Builder;
        if (stream.CanSeek)
        {
            if (stream.Length == 0)
            {
                writer.WriteStringValue(Header);
                return;
            }

            if (stream.Position == stream.Length)
            {
                throw new ArgumentException("The stream's position is the same as its length. Did you forget to rewind it?");
            }

            if (stream is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var memoryStreamBuffer))
            {
                var base64 = string.Concat(Header, Convert.ToBase64String(memoryStreamBuffer.AsSpan()));
                writer.WriteStringValue(base64);
                return;
            }

            base64Builder = new StringBuilder(Header, (int) ((stream.Length - stream.Position) * 1.37f) + Header.Length);
        }
        else
        {
            base64Builder = new StringBuilder(Header);
        }

        // 3/4 ratio
        Span<byte> byteSpan = stackalloc byte[3072];
        Span<char> charSpan = stackalloc char[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(byteSpan)) != 0)
        {
            if (!Convert.TryToBase64Chars(byteSpan[..bytesRead], charSpan, out var charsWritten))
            {
                throw new ArgumentException("The stream could not be converted to base64.");
            }

            base64Builder.Append(charSpan[..charsWritten]);
        }

        writer.WriteStringValue(base64Builder.ToString());
    }
}

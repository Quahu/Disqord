using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Disqord.Serialization.Json.Newtonsoft
{
    internal sealed class StreamConverter : JsonConverter
    {
        // This header works regardless of the actual type of the attachment.
        public const string HEADER = "data:image/jpeg;base64,";

        public static readonly StreamConverter Instance = new StreamConverter();

        private StreamConverter()
        { }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
            => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => throw new NotSupportedException();

        public override unsafe void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Also serves as a null check.
            if (!(value is Stream stream))
            {
                writer.WriteNull();
                return;
            }

            if (!stream.CanRead)
                throw new InvalidDataException("The stream is not readable.");

            StringBuilder base64Builder;
            if (!stream.CanSeek)
            {
                // If the stream isn't seekable all we can do is start reading from it.
                base64Builder = new StringBuilder(HEADER);
            }
            else
            {
                // Check if it's an empty attachment.
                if (stream.Length == 0)
                {
                    writer.WriteValue(HEADER);
                    return;
                }

                // Check if the user didn't rewind the stream.
                if (stream.Position == stream.Length)
                    throw new InvalidDataException("The stream's position is the same as its length. Did you forget to rewind it?");

                // Check if the stream is a memory stream and the underlying buffer is retrievable,
                // so we can skip the reading as all of the memory is already allocated anyways.
                if (stream is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var memoryStreamBuffer))
                {
                    var base64 = string.Concat(HEADER, Convert.ToBase64String(memoryStreamBuffer.AsSpan()));
                    writer.WriteValue(base64);
                    return;
                }

                // If the stream is seekable we can use its length and position to roughly calculate its base64 length.
                base64Builder = new StringBuilder(HEADER, (int) ((stream.Length - stream.Position) * 1.37f) + HEADER.Length);
            }

            // TODO: Do something about both not fully downloaded http streams
            //       and buffer underflowing.

            // Allocate a 3 bytes span buffer for reading data from the stream
            // and a 4 chars span buffer for the base64 encoded bytes.
            Span<byte> byteSpan = stackalloc byte[3];
            Span<char> charSpan = stackalloc char[4];
            int bytesRead;
            while ((bytesRead = stream.Read(byteSpan)) != 0)
            {
                if (!Convert.TryToBase64Chars(byteSpan.Slice(0, bytesRead), charSpan, out var charsWritten))
                    throw new InvalidDataException("The stream could not be converted to base64.");

                base64Builder.Append(charSpan.Slice(0, charsWritten));
            }

            writer.WriteValue(base64Builder.ToString());
        }
    }
}

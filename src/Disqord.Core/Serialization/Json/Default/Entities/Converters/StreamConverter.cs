using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class StreamConverter : JsonConverter
{
    // This header works regardless of the actual type of the attachment.
    public const string Header = "data:image/jpeg;base64,";

    public override bool CanRead => false;

    private readonly DefaultJsonSerializer _serializer;

    private bool _shownHttpWarning;
    private Type? _httpBaseContentType;

    public StreamConverter(DefaultJsonSerializer serializer)
    {
        _serializer = serializer;
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override unsafe void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        // This is just a null-check but because the converter is untyped we use this to also cast the object value.
        if (value is not Stream stream)
        {
            writer.WriteNull();
            return;
        }

        Guard.CanRead(stream);

        // Shows a warning for System.Net.Http content streams.
        // See CheckStreamType for more information.
        CheckStreamType(stream);

        StringBuilder base64Builder;
        if (stream.CanSeek)
        {
            // Check if it's an empty attachment.
            if (stream.Length == 0)
            {
                writer.WriteValue(Header);
                return;
            }

            // Check if the user didn't rewind the stream.
            if (stream.Position == stream.Length)
                throw new ArgumentException("The stream's position is the same as its length. Did you forget to rewind it?");

            // Check if the stream is a memory stream and the underlying buffer is retrievable,
            // so we can skip the reading as all of the memory is already allocated anyways.
            if (stream is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var memoryStreamBuffer))
            {
                var base64 = string.Concat(Header, Convert.ToBase64String(memoryStreamBuffer.AsSpan()));
                writer.WriteValue(base64);
                return;
            }

            // Because the stream is seekable we can use its length and position to roughly calculate its base64 length.
            base64Builder = new StringBuilder(Header, (int) ((stream.Length - stream.Position) * 1.37f) + Header.Length);
        }
        else
        {
            // If the stream isn't seekable all we can do is start reading from it.
            base64Builder = new StringBuilder(Header);
        }

        // Allocate a byte span buffer for reading data from the stream
        // and a char span buffer for the base64 encoded bytes (3/4 ratio).
        Span<byte> byteSpan = stackalloc byte[3072];
        Span<char> charSpan = stackalloc char[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(byteSpan)) != 0)
        {
            if (!Convert.TryToBase64Chars(byteSpan[..bytesRead], charSpan, out var charsWritten))
                throw new ArgumentException("The stream could not be converted to base64.");

            base64Builder.Append(charSpan[..charsWritten]);
        }

        writer.WriteValue(base64Builder.ToString());
    }

    // This method just checks if the user passed a System.Net.Http content stream and warns them, if they did.
    // The reasoning is that content streams are lazy, meaning code like `var stream = await HttpClient#GetStreamAsync()`
    // doesn't actually download the stream completely, as many would expect it to.
    // It's inconsistent with other GetXAsync() methods which are all content requests while that one, for some reason,
    // is only a headers request. If the user passes that stream for serialization the base64 encoding code above
    // won't work due to possible buffer underflowing. If I was to make it work with it - it'd be all synchronous, hence I'd rather just warn the user and have them both
    // acknowledge how GetStreamAsync() works and pass me a seekable stream, whether it'd be a MemoryStream or a FileStream as
    // it's going to be both more efficient as well as more reliable.
    public void CheckStreamType(Stream stream)
    {
        lock (this)
        {
            if (_shownHttpWarning || !_serializer.ShowHttpStreamsWarning)
                return;

            // Check if the type is already cached.
            if (_httpBaseContentType == null)
            {
                // The following loops get all currently loaded assemblies and find System.Net.Http,
                // in which they try to find the internal HttpBaseStream class.
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (var i = 0; i < assemblies.Length; i++)
                {
                    var assembly = assemblies[i];
                    if (assembly.GetName().Name != "System.Net.Http")
                        continue;

                    var types = assembly.GetTypes();
                    for (var j = 0; j < types.Length; j++)
                    {
                        var type = types[j];
                        if (type.Name != "HttpBaseStream")
                            continue;

                        // Cache the type for future checks.
                        _httpBaseContentType = type;
                    }
                }

                if (_httpBaseContentType == null)
                {
                    // This means that the assembly hasn't been loaded, so we assume the user simply isn't using System.Net.Http.
                    // Let's not check anything else.
                    _shownHttpWarning = true;
                    return;
                }
            }

            // Check if the given stream implements HttpBaseStream.
            if (!_httpBaseContentType.IsInstanceOfType(stream))
                return;

            _httpBaseContentType = null;
            _shownHttpWarning = true;
            _serializer.Logger.LogWarning(
                "You are passing HTTP streams directly to the API methods which is highly advised against due to buffer data underflowing for incomplete streams. " +
                "If you ignore this warning, ensure the streams are fully downloaded or copy them over to MemoryStreams. " +
                "This warning will not appear again.");
        }
    }
}

﻿using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Newtonsoft;

/// <summary>
///     Represents the default <see cref="IJsonSerializer"/>.
///     Uses Newtonsoft.Json.
/// </summary>
public class NewtonsoftJsonSerializer : IJsonSerializer
{
    /// <summary>
    ///     Gets the underlying <see cref="JsonSerializer"/>.
    /// </summary>
    public JsonSerializer UnderlyingSerializer { get; }

    internal readonly bool ShowHttpStreamsWarning;
    internal readonly ILogger Logger;

    private static readonly Encoding Utf8 = new UTF8Encoding(false);

    public NewtonsoftJsonSerializer(
        IOptions<NewtonsoftJsonSerializerConfiguration> options,
        ILogger<NewtonsoftJsonSerializer> logger)
    {
        ShowHttpStreamsWarning = options.Value.ShowHttpStreamsWarning;
        Logger = logger;

        UnderlyingSerializer = new JsonSerializer
        {
            DateParseHandling = DateParseHandling.None,
            ContractResolver = new ContractResolver(this)
        };
    }

    /// <inheritdoc/>
    public virtual TValue? Deserialize<TValue>(Stream stream)
    {
        try
        {
            using (var textReader = new StreamReader(stream, Utf8, leaveOpen: true))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                return UnderlyingSerializer.Deserialize<TValue>(jsonReader);
            }
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(isDeserialize: true, typeof(TValue), ex);
        }
    }

    /// <inheritdoc/>
    public virtual void Serialize(Stream stream, object obj, JsonSerializationOptions? options = null)
    {
        try
        {
            using (var streamWriter = new StreamWriter(stream, Utf8, leaveOpen: true))
            using (var jsonWriter = FormattingJsonWriter.Conditional(options, streamWriter))
            {
                UnderlyingSerializer.Serialize(jsonWriter, JToken.FromObject(obj, UnderlyingSerializer));

                jsonWriter.Flush();
            }
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(false, obj.GetType(), ex);
        }
    }

    /// <inheritdoc/>
    public virtual IJsonNode GetJsonNode(object? obj)
    {
        if (obj == null)
        {
            return NewtonsoftJsonNode.Create(JValue.CreateNull(), UnderlyingSerializer);
        }

        return NewtonsoftJsonNode.Create(JToken.FromObject(obj, UnderlyingSerializer), UnderlyingSerializer);
    }

    protected class FormattingJsonWriter : JsonTextWriter
    {
        private bool _shouldWriteSpace;

        public FormattingJsonWriter(TextWriter textWriter)
            : base(textWriter)
        {
            _shouldWriteSpace = true;
        }

        protected override void WriteIndentSpace()
        {
            if (!_shouldWriteSpace)
            {
                _shouldWriteSpace = true;
                return;
            }

            base.WriteIndentSpace();
        }

        public override void WriteStartObject()
        {
            if (WriteState is WriteState.Property)
            {
                WriteIndent();
                _shouldWriteSpace = false;
            }

            base.WriteStartObject();
        }

        public override void WriteStartArray()
        {
            if (WriteState is WriteState.Property)
            {
                WriteIndent();
                _shouldWriteSpace = false;
            }

            base.WriteStartArray();
        }

        public static JsonTextWriter Conditional(JsonSerializationOptions? options, TextWriter writer)
        {
            if (options?.Formatting == JsonFormatting.Indented)
            {
                var jsonWriter = new FormattingJsonWriter(writer);
                jsonWriter.Formatting = (Formatting) options.Formatting;
                if (options.Formatting == JsonFormatting.Indented)
                {
                    jsonWriter.Indentation = 4;
                }

                return jsonWriter;
            }

            return new JsonTextWriter(writer);
        }
    }
}

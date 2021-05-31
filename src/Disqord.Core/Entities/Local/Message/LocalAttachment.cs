using System;
using System.IO;

namespace Disqord
{
    /// <summary>
    ///     Represents a local attachment to be sent to Discord.
    /// </summary>
    public class LocalAttachment : ILocalConstruct, IDisposable
    {
        /// <summary>
        ///     The prefix for spoiler attachments.
        /// </summary>
        public const string SPOILER_PREFIX = "SPOILER_";

        public static LocalAttachment File(string path, string fileName = null, bool isSpoiler = false)
            => File(System.IO.File.OpenRead(path), fileName, isSpoiler);

        public static LocalAttachment File(FileStream fileStream, string fileName = null, bool isSpoiler = false)
            => new(fileStream, fileName ?? Path.GetFileName(fileStream.Name), isSpoiler);

        public static LocalAttachment Bytes(ArraySegment<byte> bytes, string fileName, bool isSpoiler = false)
            => new(new MemoryStream(bytes.Array, bytes.Offset, bytes.Count, true, true), fileName, isSpoiler);

        /// <summary>
        ///     Gets or sets the <see cref="System.IO.Stream"/> of this attachment.
        /// </summary>
        public Stream Stream { get; init; }

        /// <summary>
        ///     Gets or sets the file name of this attachment.
        /// </summary>
        public string FileName { get; init; }

        /// <summary>
        ///     Gets or sets whether this attachment should be sent as a spoiler or not.
        /// </summary>
        public bool IsSpoiler { get; init; }

        public LocalAttachment()
        { }

        public LocalAttachment(Stream stream, string fileName, bool isSpoiler = false)
        {
            Stream = stream;
            FileName = fileName;
            IsSpoiler = isSpoiler;
        }

        internal string GetFileName()
            => IsSpoiler
                ? string.Concat(SPOILER_PREFIX, FileName)
                : FileName;

        /// <summary>
        ///     Disposes of the <see cref="Stream"/>.
        /// </summary>
        public void Dispose()
            => Stream.Dispose();

        public LocalAttachment Clone()
            => MemberwiseClone() as LocalAttachment;

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (Stream == null)
                throw new InvalidOperationException("The attachment's stream must be set.");

            if (FileName == null)
                throw new InvalidOperationException("The attachment's file name must be set.");
        }
    }
}

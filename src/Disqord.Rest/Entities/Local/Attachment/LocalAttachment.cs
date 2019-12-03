using System;
using System.IO;

namespace Disqord
{
    /// <summary>
    ///     Represents a local attachment to be sent to Discord.
    /// </summary>
    public sealed class LocalAttachment : IDisposable
    {
        /// <summary>
        ///     The prefix for spoiler attachments.
        /// </summary>
        public const string SPOILER_PREFIX = "SPOILER_";

        /// <summary>
        ///     Gets the <see cref="System.IO.Stream"/> to send.
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        ///     Gets the file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     Gets whether this attachment should be sent as a spoiler or not.
        /// </summary>
        public bool IsSpoiler { get; }

        public LocalAttachment(Stream stream, string fileName, bool isSpoiler = false)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            Stream = stream;
            FileName = fileName;
            IsSpoiler = isSpoiler;
        }

        public LocalAttachment(FileStream fileStream, string fileName = null, bool isSpoiler = false)
            : this(fileStream as Stream, fileName ?? Path.GetFileName(fileStream.Name), isSpoiler)
        { }

        public LocalAttachment(string path, string fileName = null, bool isSpoiler = false)
            : this(File.OpenRead(path), fileName, isSpoiler)
        { }

        public LocalAttachment(byte[] bytes, string fileName, bool isSpoiler = false)
            : this(new MemoryStream(bytes), fileName, isSpoiler)
        { }

        public LocalAttachment(ArraySegment<byte> segment, string fileName, bool isSpoiler = false)
            : this(new MemoryStream(segment.Array, segment.Offset, segment.Count), fileName, isSpoiler)
        { }

        internal string GetFileName()
            => IsSpoiler
                ? string.Concat(SPOILER_PREFIX, FileName)
                : FileName;

        /// <summary>
        ///     Disposes of the <see cref="Stream"/>.
        /// </summary>
        public void Dispose()
            => Stream.Dispose();
    }
}

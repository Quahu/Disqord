using System;
using System.IO;
using Disqord.Serialization;

namespace Disqord
{
    /// <summary>
    ///     Represents a local attachment to be sent to Discord.
    /// </summary>
    public sealed class LocalAttachment : ILocalAttachment, IDisposable
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
        ///     Gets the filename.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     Gets whether this attachment should be sent as a spoiler or not.
        /// </summary>
        public bool IsSpoiler { get; }

        /// <summary>
        ///     Initialises a new <see cref="LocalAttachment"/> from the specified <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream"> The <see cref="System.IO.Stream"/> to send. </param>
        /// <param name="fileName"> The file name to use. </param>
        /// <param name="isSpoiler"> Whether this attachment is a spoiler. </param>
        /// <exception cref="ArgumentNullException">
        ///     The stream must not be null.
        /// </exception>
        public LocalAttachment(Stream stream, string fileName, bool isSpoiler = false)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            FileName = GetSpoilerFileName(fileName, isSpoiler);
            IsSpoiler = IsSpoiler;
        }

        /// <summary>
        ///     Initialises a new <see cref="LocalAttachment"/> from the specified <see cref="FileStream"/>.
        /// </summary>
        /// <param name="path"> The path to the file. </param>
        /// <param name="fileName"> The file name to use. </param>
        /// <param name="isSpoiler"> Whether this attachment is a spoiler. </param>
        /// <exception cref="ArgumentNullException">
        ///     The path must not be null.
        /// </exception>
        public LocalAttachment(string path, string fileName = null, bool isSpoiler = false)
            : this(File.OpenRead(path ?? throw new ArgumentNullException(nameof(path))), fileName, isSpoiler)
        { }

        /// <summary>
        ///     Initialises a new <see cref="LocalAttachment"/> from the specified <see cref="FileStream"/>.
        /// </summary>
        /// <param name="fileStream"> The <see cref="FileStream"/> to send. </param>
        /// <param name="fileName"> The file name to use. </param>
        /// <param name="isSpoiler"> Whether this attachment is a spoiler. </param>
        /// <exception cref="ArgumentNullException">
        ///     The file stream must not be null.
        /// </exception>
        public LocalAttachment(FileStream fileStream, string fileName = null, bool isSpoiler = false)
            : this(fileStream as Stream, fileName ?? Path.GetFileName(fileStream.Name), isSpoiler)
        { }

        private string GetSpoilerFileName(string fileName, bool isSpoiler)
            => isSpoiler && !fileName.StartsWith(SPOILER_PREFIX)
                ? string.Concat(SPOILER_PREFIX, fileName)
                : fileName;

        /// <summary>
        ///     Disposes of the <see cref="Stream"/>.
        /// </summary>
        public void Dispose()
            => Stream.Dispose();
    }
}

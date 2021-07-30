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
        public const string SpoilerPrefix = "SPOILER_";

        public static LocalAttachment File(string path, string fileName = null, bool isSpoiler = false, bool leaveOpen = false)
            => File(System.IO.File.OpenRead(path), fileName, isSpoiler, leaveOpen);

        public static LocalAttachment File(FileStream fileStream, string fileName = null, bool isSpoiler = false, bool leaveOpen = false)
            => new(fileStream, fileName ?? Path.GetFileName(fileStream.Name), isSpoiler, leaveOpen);

        public static LocalAttachment Bytes(ArraySegment<byte> bytes, string fileName, bool isSpoiler = false, bool leaveOpen = false)
            => new(new MemoryStream(bytes.Array, bytes.Offset, bytes.Count, true, true), fileName, isSpoiler, leaveOpen);

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

        /// <summary>
        ///     Gets or sets whether this attachment should not dispose the stream when <see cref="Dispose"/> is called.
        /// </summary>
        public bool LeaveOpen { get; init; }

        public LocalAttachment()
        { }

        public LocalAttachment(Stream stream, string fileName, bool isSpoiler = false, bool leaveOpen = false)
        {
            Stream = stream;
            FileName = fileName;
            IsSpoiler = isSpoiler;
            LeaveOpen = leaveOpen;
        }

        internal string GetFileName()
            => IsSpoiler
                ? string.Concat(SpoilerPrefix, FileName)
                : FileName;

        /// <summary>
        ///     Disposes of the <see cref="Stream"/> if <see cref="LeaveOpen"/> is <see langword="false"/>.
        /// </summary>
        public void Dispose()
        {
            if (!LeaveOpen)
                Stream.Dispose();
        }

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

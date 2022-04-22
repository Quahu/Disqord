using System;
using System.IO;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents a local attachment to be sent to Discord.
    /// </summary>
    /// <remarks>
    ///     Disposing of this type disposes of the <see cref="Stream"/> unless <see cref="LeaveOpen"/> is set to true.
    /// </remarks>
    public class LocalAttachment : ILocalConstruct, IDisposable
    {
        /// <summary>
        ///     The prefix for spoiler attachments.
        /// </summary>
        public const string SpoilerPrefix = "SPOILER_";

        /// <summary>
        ///     Instantiates a new <see cref="LocalAttachment"/> using the path to the file.
        /// </summary>
        /// <remarks>
        ///     Short for <see cref="System.IO.File.OpenRead"/>.
        /// </remarks>
        /// <param name="path"> The path to the file. </param>
        /// <param name="fileName"> The name of the file. If not specified, will default to what the file is called in the system. </param>
        /// <returns>
        ///     The <see cref="LocalAttachment"/> instance.
        /// </returns>
        public static LocalAttachment File(string path, string fileName = null)
        {
            var stream = System.IO.File.OpenRead(path);
            return File(stream, fileName);
        }

        /// <summary>
        ///     Instantiates a new <see cref="LocalAttachment"/> using a file stream.
        /// </summary>
        /// <remarks>
        ///     Short for <see cref="System.IO.File.OpenRead"/>.
        /// </remarks>
        /// <param name="fileStream"> The file stream. </param>
        /// <param name="fileName"> The name of the file. If not specified, will default to what the file is called in the system. </param>
        /// <returns>
        ///     The <see cref="LocalAttachment"/> instance.
        /// </returns>
        public static LocalAttachment File(FileStream fileStream, string fileName = null)
        {
            fileName ??= Path.GetFileName(fileStream.Name);
            return new(fileStream, fileName);
        }

        /// <summary>
        ///     Instantiates a new <see cref="LocalAttachment"/> using a byte array.
        /// </summary>
        /// <remarks>
        ///     Short for <see cref="System.IO.File.OpenRead"/>.
        /// </remarks>
        /// <param name="bytes"> The byte array. </param>
        /// <param name="fileName"> The name of the file. </param>
        /// <returns>
        ///     The <see cref="LocalAttachment"/> instance.
        /// </returns>
        public static LocalAttachment Bytes(ArraySegment<byte> bytes, string fileName)
        {
            var stream = new MemoryStream(bytes.Array!, bytes.Offset, bytes.Count, true, true);
            return new(stream, fileName);
        }

        // /// <summary>
        // ///     Gets or sets the ID of this attachment.
        // /// </summary>
        // /// <remarks>
        // ///     This is not required, but can be set for if you want to later delete the
        // ///     attachment on an existing message.
        // ///     E.g. setting this to <c>42</c> lets you easily delete it later by just removing the attachment with ID <c>42</c>.
        // /// </remarks>
        // public Optional<Snowflake> Id { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="System.IO.Stream"/> of this attachment.
        /// </summary>
        public Optional<Stream> Stream { get; set; }

        /// <summary>
        ///     Gets or sets the file name of this attachment.
        /// </summary>
        public Optional<string> FileName { get; set; }

        /// <summary>
        ///     Gets or sets the description of this attachment.
        /// </summary>
        public Optional<string> Description { get; set; }

        /// <summary>
        ///     Gets or sets whether this attachment should be sent as a spoiler.
        /// </summary>
        public Optional<bool> IsSpoiler { get; set; }

        /// <summary>
        ///     Gets or sets whether this attachment should not dispose the stream when <see cref="Dispose"/> is called.
        /// </summary>
        public bool LeaveOpen { get; set; }

        public LocalAttachment()
        { }

        public LocalAttachment(Stream stream, string fileName, bool isSpoiler = false, bool leaveOpen = false)
        {
            Stream = stream;
            FileName = fileName;
            IsSpoiler = isSpoiler;
            LeaveOpen = leaveOpen;
        }

        /// <summary>
        ///     Disposes of the <see cref="Stream"/> if <see cref="LeaveOpen"/> is <see langword="false"/>.
        /// </summary>
        public virtual void Dispose()
        {
            if (!LeaveOpen && Stream.TryGetValue(out var stream))
            {
                stream?.Dispose();
            }
        }

        public virtual LocalAttachment Clone()
            => MemberwiseClone() as LocalAttachment;

        object ICloneable.Clone()
            => Clone();
    }
}

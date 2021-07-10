namespace Disqord
{
    /// <summary>
    ///     Represents a text guild channel.
    /// </summary>
    public interface ITextChannel : INestableChannel, IMessageChannel, IMentionable, ITaggable
    {
        /// <summary>
        ///     Gets the topic of this channel.
        /// </summary>
        string Topic { get; }

        /// <summary>
        ///     Gets whether this channel is NSFW.
        /// </summary>
        bool IsNsfw { get; }

        /// <summary>
        ///     Gets the slow mode in seconds of this channel.
        /// </summary>
        int Slowmode { get; }

        /// <summary>
        ///     Gets whether this channel is a news channel.
        /// </summary>
        bool IsNews { get; }

        /// <summary>
        ///     Gets whether this channel is a store channel.
        /// </summary>
        bool IsStore { get; }
    }
}

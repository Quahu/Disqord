namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a member's streaming activity, e.g. a Twitch or YouTube stream.
    /// </summary>
    public interface IStreamingActivity : IActivity
    {
        /// <summary>
        ///     Gets the stream URL of this activity.
        /// </summary>
        string Url { get; }
    }
}

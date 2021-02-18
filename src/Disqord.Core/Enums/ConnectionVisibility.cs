namespace Disqord
{
    /// <summary>
    ///     Represents the visibility of a user's connection.
    /// </summary>
    public enum ConnectionVisibility : byte
    {
        /// <summary>
        ///     Invisible to everyone except the user themselves.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Visible to everyone.
        /// </summary>
        Everyone = 1
    }
}

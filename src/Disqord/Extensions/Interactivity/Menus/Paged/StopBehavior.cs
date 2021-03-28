namespace Disqord.Extensions.Interactivity.Menus.Paged
{
    /// <summary>
    ///     Represents the behavior applied to the <see cref="PagedMenu.Message"/> by the default stop button.
    /// </summary>
    public enum StopBehavior
    {
        /// <summary>
        ///     Nothing will happen with the message.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The reactions on the message will be cleared.
        /// </summary>
        ClearReactions = 1,

        /// <summary>
        ///     The message will be deleted.
        /// </summary>
        DeleteMessage = 2
    }
}
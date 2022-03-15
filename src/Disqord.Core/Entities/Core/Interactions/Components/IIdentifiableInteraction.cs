namespace Disqord
{
    /// <summary>
    ///     Represents an interaction that can be identified with a custom ID
    /// </summary>
    public interface IIdentifiableInteraction : IInteraction
    {
        /// <summary>
        ///     Gets the custom ID of the component of this interaction.
        /// </summary>
        string CustomId { get; }
    }
}

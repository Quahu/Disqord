namespace Disqord
{
    public interface IComponentInteraction : IInteraction
    {
        /// <summary>
        ///     Gets the component ID of this interaction.
        /// </summary>
        string ComponentId { get; }

        /// <summary>
        ///     Gets the component type of this interaction.
        /// </summary>
        ComponentType ComponentType { get; }

        /// <summary>
        ///     Gets the message of this interaction.
        /// </summary>
        IUserMessage Message { get; }
    }
}

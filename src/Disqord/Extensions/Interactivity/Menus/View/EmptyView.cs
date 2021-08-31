namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents an empty implementation of <see cref="ViewBase"/>.
    ///     This type can be used for views that only contain dynamically generated components.
    /// </summary>
    public class EmptyView : ViewBase
    {
        /// <inheritdoc/>
        /// <summary>
        ///     Instantiates a new <see cref="EmptyView"/> with the specified template message.
        /// </summary>
        public EmptyView(LocalMessage templateMessage)
            : base(templateMessage)
        { }
    }
}

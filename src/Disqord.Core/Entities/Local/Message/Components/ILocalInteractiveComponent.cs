namespace Disqord
{
    public interface ILocalInteractiveComponent : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the custom ID of this component.
        /// </summary>
        Optional<string> CustomId { get; set; }
    }
}

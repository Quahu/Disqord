namespace Disqord
{
    public interface ILocalInteractiveComponent : ILocalConstruct
    {
        public const int MaxCustomIdLength = 100;

        /// <summary>
        ///     Gets or sets the custom ID of this component.
        /// </summary>
        string CustomId { get; set; }
    }
}

using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents a local entity that can be identified with a custom ID.
    /// </summary>
    public interface ILocalCustomIdentifiableEntity : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the custom ID of this local entity.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        Optional<string> CustomId { get; set; }
    }
}

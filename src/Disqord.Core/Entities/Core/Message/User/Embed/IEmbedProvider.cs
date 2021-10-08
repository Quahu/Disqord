namespace Disqord
{
    /// <summary>
    ///     Represents the embed provider of an embed.
    /// </summary>
    public interface IEmbedProvider : IEntity, INamableEntity
    {
        /// <summary>
        ///     Gets the URL of this provider.
        ///     Returns <see langword="null"/> if not present.
        /// </summary>
        string Url { get; }
    }
}

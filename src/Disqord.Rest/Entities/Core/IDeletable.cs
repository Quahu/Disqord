using System.Threading.Tasks;

namespace Disqord
{
    /// <summary>
    ///     Represents a deletable Discord entity.
    /// </summary>
    public interface IDeletable : IDiscordEntity
    {
        /// <summary>
        ///     Deletes this <see cref="IDeletable"/> entity.
        /// </summary>
        /// <param name="options"> The request options. </param>
        Task DeleteAsync(RestRequestOptions options = null);
    }
}

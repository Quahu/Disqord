using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an account of a guild integration.
    /// </summary>
    public interface IIntegrationAccount : INamable, IJsonUpdatable<IntegrationAccountJsonModel>
    {
        /// <summary>
        ///     Gets the identifier of this account.
        /// </summary>
        public string Id { get; }
    }
}

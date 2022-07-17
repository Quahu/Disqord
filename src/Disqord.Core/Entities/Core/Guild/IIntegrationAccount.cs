using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an account of a guild integration.
/// </summary>
public interface IIntegrationAccount : INamableEntity, IJsonUpdatable<IntegrationAccountJsonModel>
{
    /// <summary>
    ///     Gets the identifier of this account.
    /// </summary>
    string Id { get; }
}

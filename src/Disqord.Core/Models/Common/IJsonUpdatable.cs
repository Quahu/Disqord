using System.ComponentModel;
using Disqord.Serialization.Json;

namespace Disqord.Models;

/// <summary>
///     Represents an entity that can be updated from a <typeparamref name="TModel"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IJsonUpdatable<in TModel>
    where TModel : JsonModel
{
    /// <summary>
    ///     Updates this entity from the specified <typeparamref name="TModel"/>.
    /// </summary>
    /// <param name="model"> The model to update from. </param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Update(TModel model);
}

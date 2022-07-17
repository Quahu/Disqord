using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents a short-lived client entity that wraps a JSON model.
/// </summary>
/// <typeparam name="TModel"> The type of the JSON model. </typeparam>
public interface ITransientEntity<TModel> : IJsonUpdatable<TModel>, IEntity
    where TModel : JsonModel
{
    /// <summary>
    ///     Gets the JSON model this entity wraps.
    /// </summary>
    TModel Model { get; }

    /// <summary>
    ///     Throws <see cref="NotSupportedException"/> as transient entities do not support updates.
    /// </summary>
    /// <param name="model"> <inheritdoc/> </param>
    void IJsonUpdatable<TModel>.Update(TModel model)
    {
        throw new NotSupportedException("Transient entities do not support updates.");
    }
}

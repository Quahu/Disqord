using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents a short-lived client entity that wraps a JSON model.
/// </summary>
/// <typeparam name="TModel"> <inheritdoc cref="ITransientEntity{TModel}"/> </typeparam>
public interface ITransientClientEntity<TModel> : ITransientEntity<TModel>, IClientEntity
    where TModel : JsonModel
{ }
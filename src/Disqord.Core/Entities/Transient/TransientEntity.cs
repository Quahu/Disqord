using Disqord.Serialization.Json;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="ITransientEntity{TModel}"/>
public abstract class TransientEntity<TModel> : ITransientEntity<TModel>
    where TModel : JsonModel
{
    /// <inheritdoc/>
    public TModel Model { get; }

    /// <summary>
    ///     Instantiates a new <see cref="TransientEntity{TModel}"/> with the specified JSON model.
    /// </summary>
    /// <param name="model"> The JSON model of this entity. </param>
    protected TransientEntity(TModel model)
    {
        Guard.IsNotNull(model);

        Model = model;
    }

    public override string ToString()
    {
        return this.GetString();
    }
}

using Disqord.Serialization.Json;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="ITransientClientEntity{TModel}"/>
public abstract class TransientClientEntity<TModel> : TransientEntity<TModel>, ITransientClientEntity<TModel>
    where TModel : JsonModel
{
    /// <inheritdoc/>
    public IClient Client { get; }

    /// <summary>
    ///     Instantiates a new <see cref="TransientEntity{TModel}"/> with the specified client and JSON model.
    /// </summary>
    /// <param name="client"> The client of this entity. </param>
    /// <param name="model"> The JSON model of this entity. </param>
    protected TransientClientEntity(IClient client, TModel model)
        : base(model)
    {
        Guard.IsNotNull(client);

        Client = client;
    }
}
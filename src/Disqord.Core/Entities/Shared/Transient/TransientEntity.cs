using System;
using Disqord.Serialization.Json;

namespace Disqord
{
    /// <inheritdoc cref="ITransientEntity{TModel}"/>
    public abstract class TransientEntity<TModel> : Entity, ITransientEntity<TModel>
        where TModel : JsonModel
    {
        /// <inheritdoc/>
        public TModel Model { get; }

        /// <summary>
        ///     Instantiates a new <see cref="TransientEntity{TModel}"/> with the specified client and JSON model.
        /// </summary>
        /// <param name="client"> The managing client. </param>
        /// <param name="model"> The JSON model. </param>
        protected TransientEntity(IClient client, TModel model)
            : base(client)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
        }
    }
}

using Disqord.Serialization.Json;

namespace Disqord.Gateway;

public abstract class TransientGatewayClientEntity<TModel> : TransientClientEntity<TModel>, IGatewayClientEntity
    where TModel : JsonModel
{
    /// <inheritdoc/>
    public new IGatewayClient Client => (base.Client as IGatewayClient)!;

    protected TransientGatewayClientEntity(IClient client, TModel model)
        : base(client, model)
    { }
}

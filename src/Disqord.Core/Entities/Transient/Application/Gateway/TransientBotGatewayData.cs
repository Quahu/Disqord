using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IBotGatewayData"/>/>
public class TransientBotGatewayData : TransientClientEntity<BotGatewayJsonModel>, IBotGatewayData
{
    /// <inheritdoc/>
    public string Url => Model.Url;

    /// <inheritdoc/>
    public int RecommendedShardCount => Model.Shards;

    /// <inheritdoc/>
    public IBotGatewaySessionData Sessions => _sessions ??= new TransientBotGatewaySessionData(Client, Model.SessionStartLimit);

    private TransientBotGatewaySessionData? _sessions;

    public TransientBotGatewayData(IClient client, BotGatewayJsonModel model)
        : base(client, model)
    { }
}

using Disqord.Models;

namespace Disqord.Gateway
{
    /// <inheritdoc cref="IBotGatewayData"/>/>
    public class TransientBotGatewayData : TransientEntity<BotGatewayJsonModel>, IBotGatewayData
    {
        /// <inheritdoc/>
        public string Url => Model.Url;

        /// <inheritdoc/>
        public int RecommendedShardCount => Model.Shards;

        /// <inheritdoc/>
        public IBotGatewaySessionData Sessions
        {
            get
            {
                if (_sessions == null)
                    _sessions = new TransientBotGatewaySessionData(Client, Model.SessionStartLimit);

                return _sessions;
            }
        }
        private TransientBotGatewaySessionData _sessions;

        public TransientBotGatewayData(IClient client, BotGatewayJsonModel model)
            : base(client, model)
        { }
    }
}
